using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendForFrontend.Models.EFModels;
using NuGet.Protocol.Plugins;
using BackendForFrontend.Models.Dtos;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.Json;
using System.Text;

namespace BackendForFrontend.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class EmailSendController : ControllerBase
    {
        private readonly EmailService _emailService;
        private readonly AppDbContext _context;

        public EmailSendController(AppDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }


        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.ResetToken == resetPasswordDto.Token && m.ResetTokenExpiration > DateTime.UtcNow);

            if (member == null)
            {
                return BadRequest("無效的重設令牌或令牌已過期。");
            }

            // 更新密碼
            member.Password = resetPasswordDto.NewPassword; // 確保這裡使用加密存儲密碼
            member.ResetToken = null; // 清除重設令牌
            member.ResetTokenExpiration = null; // 清除令牌過期時間

            await _context.SaveChangesAsync();

            return Ok("密碼已成功重設。");
        }



        [HttpPost("Register")]
        public async Task<ActionResult<Member>> PostMember([FromBody] MemberRegistrationDto registrationDto)
        {
            var existingMember = await _context.Members
                .AnyAsync(m => m.Email == registrationDto.Email);
            if (existingMember)
            {
                return BadRequest("此信箱已被註冊過了");
            }

            var newMember = new Member
            {
                Name = registrationDto.Name,
                Gender = registrationDto.Gender,
                DateOfBirth = registrationDto.DateOfBirth,
                Email = registrationDto.Email,
                Password = registrationDto.Password, 
                Address = registrationDto.Address,
                PhoneNumber = registrationDto.PhoneNumber,
                MembersLevel = "1"

            };

            // 生成驗證碼
            var verificationCode = GenerateVerificationCode();
            newMember.VerificationCode = verificationCode;
            var value = 300;
            newMember.VerificationCodeExpiration = Convert.ToInt32(value);
            // ...保存驗證碼...

            _context.Members.Add(newMember);
            await _context.SaveChangesAsync();

            // 發送我的驗證碼
            var subject = "安安你好！我們是某某書局！";
            var message = $"你的驗證碼是：{verificationCode}，如果你沒有註冊請忽略此信";
            await _emailService.SendEmailAsync(newMember.Email, subject, message);

            return CreatedAtAction(nameof(GetMember), new { id = newMember.Id }, newMember);
        }

        [HttpPost("AAA")]
        public async Task<IActionResult> ResendVerificationCode(string email)
        {
            var member = await _context.Members.FirstOrDefaultAsync(m => m.Email == email);
            if (member == null)
            {
                return BadRequest("請重新再輸入。");
            }

            // 生成新的验证码并设置过期时间
            member.VerificationCode = GenerateVerificationCode();
            member.VerificationCodeExpiration = 9999;

            // 更新数据库
            _context.Entry(member).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // 重新發送驗證碼
            var subject = "安安你好！我們是某某書局！你看起來按下再次發送驗證碼囉";
            var message = $"你的驗證碼是 {member.VerificationCode}";
            await _emailService.SendEmailAsync(member.Email, subject, message);


            return Ok("汪汪");
        }

        // DTO for forgot password request
        public class ForgotPasswordDto
        {
            public string Email { get; set; }
        }

        [HttpPost("ForgotPassword")]
        // 假定是在ForgotPassword方法中實現
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            var member = await _context.Members.FirstOrDefaultAsync(m => m.Email == forgotPasswordDto.Email);
            if (member == null)
            {
                return NotFound("找不到對應的電子郵件地址。");
            }

            var resetToken = GenerateResetToken();
            member.ResetToken = resetToken;
            member.ResetTokenExpiration = DateTime.UtcNow.AddDays(1); // 1天後過期

            await _context.SaveChangesAsync();

            var subject = "密碼重置請求";
            var resetLink = $"http://localhost:5173/resetpassword/{resetToken}"; 
            var message = $"請點擊以下鏈接重置密碼：{resetLink}";
            await _emailService.SendEmailAsync(member.Email, subject, message);

            return Ok("密碼重置郵件已發送。");
        }


        private string GenerateResetToken()
        {
            // 生成一個隨機令牌，用於密碼重置
            return Guid.NewGuid().ToString();
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return member;
        }
        private string GenerateVerificationCode()
        {
            return new Random().Next(100000, 999999).ToString();
        }
    }
    //DTO專區
    public class ResetPasswordDto
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
    public class MemberRegistrationDto
    {
        public string Name { get; set; }
        public bool Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]

    //都通用的驗證碼
    public class CaptchaController : ControllerBase
    {
        private readonly IDistributedCache _cache;
        public CaptchaController(IDistributedCache cache)
        {
            _cache = cache;
        }
        private const string _content = "2345679ACDEFGHJKLMNQPRSWXYZ";//把數字0跟英文O刪掉，因為很擾民= =

        [HttpGet]
        [Route("GetCaptcha")]
        public async Task<IActionResult> GetCaptcha()
        {
            
            var captchaCode = new string[4];
            var drawFont = new Font("Arial", 12); // 字形
            var drawBrush = new SolidBrush(Color.White); // 顏色
            var rdm = new Random();

            // 隨機產生4個文字
            for (var i = 0; i < 4; i++)
            {
                captchaCode[i] = _content[rdm.Next(_content.Length)].ToString();
            }
            // 建立圖像
            var picture = new Bitmap(80, 30);
            var grap = Graphics.FromImage(picture);
            grap.Clear(Color.Black); // 設置背景色

            for (var i = 0; i < 4; i++)
            {
                var p = new Point(i * 20, 0);
                grap.DrawString(captchaCode[i], drawFont, drawBrush, p);
            }

            // 繪製干擾點
            for (var i = 0; i < 200; i++)
            {
                var p = new Point(rdm.Next(picture.Width), rdm.Next(picture.Height));
                picture.SetPixel(p.X, p.Y, Color.Pink);
            }

            var SaveCaptcha = string.Concat(captchaCode);
           
           //把驗證碼轉成Bytes
            byte[] captchaCodeBytes = Encoding.UTF8.GetBytes(SaveCaptcha);
            //產生針對使用者的SessionId
            string sessionId = HttpContext.Session.Id;
            string cacheKey = $"Captcha_{sessionId}";
            //把驗證碼存進對應 SessionId 的 Session
            await _cache.SetAsync(cacheKey, captchaCodeBytes, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            }) ;
            // 返回圖片
            using (var ms = new MemoryStream())
            {
                picture.Save(ms, ImageFormat.Png);
                return Ok(new {File= File(ms.ToArray(), "image/png") , CacheKey =cacheKey });
            }
        }
    }
}
   


    [Route("api/[controller]")]
    [ApiController]
    //會員登入
    public class MemberLoginController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtHelpers _jwtHelpers;
        private readonly IDistributedCache _cache;
    public MemberLoginController(AppDbContext context, JwtHelpers jwtHelpers, IDistributedCache cache)
        {
            _context = context;
            _jwtHelpers = jwtHelpers;
            _cache = cache;
        }


    [Authorize]
    [HttpGet("member-info")]
    public async Task<IActionResult> GetMemberInfo()
    {
        // 從 JWT 中獲取用戶名或用戶 ID
        var username = User.Identity.Name; // 假設你的 JWT 存儲了用戶名
        
        // 根據用戶名從資料庫中查詢用戶
        var member = await _context.Members
                                   .FirstOrDefaultAsync(m => m.Email == username);

        if (member == null)
        {
            return NotFound("用戶不存在");
        }

        // 創建一個 DTO 來返回用戶資料，避免直接返回資料庫實體對象
        var memberInfoDto = new MemberInfoDto
        {
            Id = member.Id,
            Name = member.Name,
            Email = member.Email,
            Address = member.Address,
            DateOfBirth = member.DateOfBirth,
            PhoneNumber = member.PhoneNumber,

            // 可以添加更多需要返回的欄位
        };

        return Ok(memberInfoDto);
    }

    // DTO 類用於返回會員資訊
    public class MemberInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        // 可以添加更多欄位
    }



    [HttpPost]
    public async Task<ActionResult<Member>> Login([FromBody] MemberLoginRequest memberloginRequest)
    {
        var member = await _context.Members
            .FirstOrDefaultAsync(m => m.Email == memberloginRequest.Email && m.Password == memberloginRequest.Password);

        if (member == null)
        {
            return Unauthorized("帳號或密碼有誤");
        }
        
       
        var serializedStoredCaptchaBytes = await _cache.GetAsync(memberloginRequest.CacheKey);
        if (serializedStoredCaptchaBytes == null) {
            return Unauthorized("驗證碼失效");
        }

        var serializedStoredCaptcha = Encoding.UTF8.GetString(serializedStoredCaptchaBytes);

        if (memberloginRequest.Captcha != serializedStoredCaptcha)
        {
            return Unauthorized("驗證碼錯誤" );
        }
        var token = _jwtHelpers.GenerateToken(member.Email);
            return Ok(new { message= "成功登入",token = token});
        }
    [HttpPost("CodeCheck")]
    public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeDto verifyCodeDto)
    {
        var member = await _context.Members.FirstOrDefaultAsync(m => m.Email == verifyCodeDto.Email);
        if (member == null)
        {
            return BadRequest("請你重新再輸入信箱。");
        }

        // 检查验证码是否匹配且未过期
        int secondsSinceCreation = (int)(DateTime.UtcNow - member.CreatedAt).TotalSeconds;
        bool codeNotExpired = member.VerificationCodeExpiration.HasValue &&
                                secondsSinceCreation <= member.VerificationCodeExpiration.Value;

        if (member.VerificationCode == verifyCodeDto.Code && codeNotExpired)
        {
            // 可选：更新用户的邮箱验证状态
            member.EmailVerified = true;
            // _context.Entry(member).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok("恭喜你完成驗證！");
        }
        else
        {
            return BadRequest("驗證碼錯誤或驗證碼已過期。");
        }
    }


    public class VerifyCodeDto
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}

    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MembersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Members
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        {
            if (_context.Members == null)
            {
                return NotFound();
            }
            return await _context.Members.ToListAsync();
        }

        // GET: api/Members/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(int id)
        {
            if (_context.Members == null)
            {
                return NotFound();
            }
            var member = await _context.Members.FindAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            return member;
        }

        // PUT: api/Members/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMember(int id, Member member)
        {
            if (id != member.Id)
            {
                return BadRequest();
            }

            _context.Entry(member).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // POST: api/Members
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Member>> PostMember(Member member)
        {
            if (_context.Members == null)
            {
                return Problem("Entity set 'AppDbContext.Members'  is null.");
            }
            _context.Members.Add(member);
            await _context.SaveChangesAsync();



            return CreatedAtAction("GetMember", new { id = member.Id }, member);
        }

        // DELETE: api/Members/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            if (_context.Members == null)
            {
                return NotFound();
            }
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    // PUT: api/Members/5/MemberEdit
    [HttpPut("{id}/MemberEdit")]
    public async Task<IActionResult> MemberEdit(int id, MemberDto member)
    {
        if (id != member.Id)
        {
            return BadRequest("完蛋啦");
        }

        var memberInDb = await _context.Members.FindAsync(id);
        if (memberInDb == null)
        {
            return NotFound("幹你是誰");
        }

        // 更新會員資料
        memberInDb.Name = member.Name;
        memberInDb.DateOfBirth = member.DateOfBirth;
        memberInDb.Email = member.Email;
        memberInDb.Address = member.Address;
        memberInDb.PhoneNumber = member.PhoneNumber;

        // 可以根據需要更新更多的欄位

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MemberExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent(); // 或者返回更新後的會員資料
    }


    private bool MemberExists(int id)
        {
            return (_context.Members?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        //[HttpPost("CodeCheck")]
        //public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeDto verifyCodeDto)
        //{
        //    var member = await _context.Members.FirstOrDefaultAsync(m => m.Email == verifyCodeDto.Email);
        //    if (member == null)
        //    {
        //        return BadRequest("請你重新再輸入信箱。");
        //    }

        //    // 检查验证码是否匹配且未过期
        //    int secondsSinceCreation = (int)(DateTime.UtcNow - member.CreatedAt).TotalSeconds;
        //    bool codeNotExpired = member.VerificationCodeExpiration.HasValue &&
        //                            secondsSinceCreation <= member.VerificationCodeExpiration.Value;

        //    if (member.VerificationCode == verifyCodeDto.Code && codeNotExpired)
        //    {
        //        // 可选：更新用户的邮箱验证状态
        //        member.EmailVerified = true;
        //        // _context.Entry(member).State = EntityState.Modified;
        //        await _context.SaveChangesAsync();
        //        return Ok("恭喜你完成驗證！");
        //    }
        //    else
        //    {
        //        return BadRequest("驗證碼錯誤或驗證碼已過期。");
        //    }
        //}

        //public class VerifyCodeDto
        //{
        //    public string Email { get; set; }
        //    public string Code { get; set; }
        //}
    }

    




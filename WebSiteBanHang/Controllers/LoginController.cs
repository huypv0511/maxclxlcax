using CaptchaMvc.HtmlHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    public class LoginController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Menu()
        {
            var LstSP = db.SanPhams;
            ViewBag.ListSP = LstSP;
            return View();
        }


        public ActionResult MenuPartial()
        {
            var LstSP = db.SanPhams;
            ViewBag.tabCount = 5;           //đặt 5 ở đây để test code inside, nếu cần thì gọi query lấy số loại dishes
            ViewBag.ListSP = LstSP;
            return PartialView(LstSP);
        }
        [HttpGet]
        public ActionResult Register()
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            return View();
        }

        [HttpPost]
        public ActionResult Register(ThanhVien tv)
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            //kiểm tra capcha hợp lệ
            if (this.IsCaptchaValid("Capcha is valid"))
            {
                
                ViewBag.ThongBao = "Success";
                //thêm khách hàng vào csdl
                if (ModelState.IsValid)
                {
                    var passWord = tv.MatKhau;
                    var checkpassWord = tv.NhapLaiMatKhau;
                    //if (passWord.Equals(checkpassWord))
                    //{
        
                    //}
                    //else 
                    //{
                    //    ModelState.AddModelError("", "Password does not match");
                    //}
                    var username = db.ThanhViens.SingleOrDefault(m => m.TaiKhoan == tv.TaiKhoan);
                    var email = db.ThanhViens.SingleOrDefault(m => m.Email.Equals(tv.Email));
                    // kiểm tra tài khoản trùng hay không
                    if (username != null)
                    {
                        ModelState.AddModelError("", "Tài khoản đã được sử dụng");
                    }
                    // kiểm tra email trùng hay không
                    else if (email != null)
                    {
                        ModelState.AddModelError("", "Email đã được sử dụng");
                    }
                    else
                    {   
                        // add vào database
                        var user = new ThanhVien();
                        user.HoTen = tv.HoTen;
                        user.KhachHangs = tv.KhachHangs;
                        user.Email = tv.Email;
                        user.DiaChi = tv.DiaChi;
                        user.CauHoi = tv.CauHoi;
                        user.CauTraLoi = tv.CauTraLoi;
                        user.BinhLuans = tv.BinhLuans;
                        user.MatKhau = tv.MatKhau;
                        user.SoDienThoai = tv.SoDienThoai;
                        user.TaiKhoan = tv.TaiKhoan;
                        db.ThanhViens.Add(user);
                        db.SaveChanges();
                        return RedirectToAction("DangNhap", "Home");
                    }
                }
                //db.ThanhViens.Add(tv);
                //db.SaveChanges();
                //return View();
                return RedirectToAction("Index","Login");
            }
            ViewBag.ThongBao = "Wrong Capcha";

            return View();
        }
        //load cau hỏi bí mật
        public List<string> LoadCauHoi()
        {
            List<string> lstCauHoi = new List<string>();
            lstCauHoi.Add("Do you have a lover?");
            lstCauHoi.Add("What's your lover's name?");
            lstCauHoi.Add("How many ex-lovers do you have?");
            return lstCauHoi;

        }
    }


}
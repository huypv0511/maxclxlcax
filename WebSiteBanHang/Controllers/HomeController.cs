using CaptchaMvc.HtmlHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        // GET: SanPham
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            //lấy dữ liệu nạp vào model(sản phẩm)
            var lstSanPhamM = db.SanPhams.Where(n => n.MaLoaiSP == 1 && n.Moi == 1);
            //gán vào viewbag
            ViewBag.ListSP = lstSanPhamM;
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
        public ActionResult DangKy()
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(ThanhVien tv)
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            //kiểm tra capcha hợp lệ
            if (this.IsCaptchaValid("Capcha is not valid"))
            {
                ViewBag.ThongBao = "Success";
                //thêm khách hàng vào csdl
                db.ThanhViens.Add(tv);
                db.SaveChanges();
                return View();
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
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(ThanhVien tv1)
        {
            try
            {
                //kiểm tra tên đăng nhập và mật khẩu
                //string sTaiKhoan = f["txtTenDangNhap"].ToString();
                //string sMatKhau = f["txtMatKhau"].ToString();
                string sTaiKhoan = tv1.Email;
                string sMatKhau = tv1.MatKhau;
                ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.Email == sTaiKhoan && n.MatKhau == sMatKhau);
                if (tv != null)
                {
                    Session["TaiKhoan"] = tv;
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                ViewBag.BaoLoi = "Tên Không Đúng";
            }

            return RedirectToAction("Index");
        }

        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            return RedirectToAction("Index");
        }


    }

}
using System.Linq;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: SanPham
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult SanPham1()
        {
            //lấy dữ liệu nạp vào model(sản phẩm)
            var lstSanPhamM = db.SanPhams.Where(n => n.MaLoaiSP == 1 && n.Moi == 1);
            //gán vào viewbag
            ViewBag.ListSP = lstSanPhamM;
            return View();
        }
        public ActionResult SanPham2()
        {
            //lấy dữ liệu nạp vào model(sản phẩm)
            var lstSanPhamM = db.SanPhams.Where(n => n.MaLoaiSP == 1 && n.Moi == 1);
            ViewBag.ListSP = lstSanPhamM;
            return View();
        }
        //tạo partialview
        [ChildActionOnly]
        public ActionResult SanPhamPartial()
        {
            //lấy dữ liệu nạp vào model(sản phẩm)
            var lstSanPhamM = db.SanPhams.Where(n => n.MaLoaiSP == 1 && n.Moi == 1);
            return PartialView(lstSanPhamM);
            //return PartialView();
        }

        // lấy info sp để hiện ở modal add to cart
        [HttpPost]
        public ActionResult SanPhamDetail(int? maSp)
        {
            var sanPham = db.SanPhams.Single(n => n.MaSP == maSp);
            return PartialView(sanPham);
        }

    }
}

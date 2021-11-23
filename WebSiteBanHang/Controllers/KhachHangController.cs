using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    public class KhachHangController : Controller
    {
        // GET: KhachHang
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            // truy vấn dữ liệu thông qua câu lệnh
            // đối lstKH sẽ lấy toàn bộ dữ liệu từ bản khách hàng
            // cách 1 : lấy dữ liệu từ 1 danh sách khách hàng
            // var lstKH = from KH in db.KhachHangs select KH;
            // cách 2: dùng phương thức hỗ trợ sẵn
            var lstKH = db.KhachHangs;
            return View(lstKH);
        }
        public ActionResult TruyVan1DoiTuong()
        {
            // truy vấn 1 đối tượng bằng câu lệnh truy vấn
            // bước 1 lấy ra danh sách khách hàng
            var lstKH = from kh in db.KhachHangs where kh.MaKH == 2 select kh;
            // bước 2
            //KhachHang khang = lstKH.FirstOrDefault();
            //lấy đối tượng khách hàng dựa trên phương thức hỗ trợ
            KhachHang khang = db.KhachHangs.SingleOrDefault(n => n.MaKH == 2);
            return View(khang);
        }
        public ActionResult SortDuLieu()
        {
            List<KhachHang> lstKH = db.KhachHangs.OrderBy(n => n.TenKH).ToList();
            return View(lstKH);
        }
        public ActionResult GroupDuLieu()
        {
            //group dữ liệu
            List<ThanhVien> lstKH = db.ThanhViens.OrderByDescending(n => n.TaiKhoan).ToList();
            return View(lstKH);
        }
    }
}

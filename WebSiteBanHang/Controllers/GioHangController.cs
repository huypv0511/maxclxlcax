using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    public class GioHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();




        //lấy giỏ hàng
        public List<itemGioHang> LayGioHang()
        {
            //giỏ hàng đã tồn tại
            List<itemGioHang> lstGioHang = Session["GioHang"] as List<itemGioHang>;
            if (lstGioHang == null)
            {
                //nếu session giỏ hàng ko tồn tại thì khởi tạo giỏ hàng
                lstGioHang = new List<itemGioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        // thêm giỏ hàng thông thường (load lại trang)
        public ActionResult ThemGioHang(int? MaSP, bool is12, int soLuong, string strURL)
        {
            //kiểm tra  sản phẩm  có tồn tại trong cơ sở dữ liệu  hay không
            Debug.WriteLine("invoked giohang with parameter MaSP " + MaSP);
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            Debug.WriteLine(sp.ToString());
            if (sp == null)
            {
                //trang đường dẫn không hợp lệ
                Response.StatusCode = 404;
                return null;
            }
            // lấy giỏ hàng
            List<itemGioHang> lstGioHang = LayGioHang();
            // trường hợp 1 nếu sản phẩm đã tồn tại trong giỏ hàng
            itemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP && n.Is12 == is12); // tìm theo mã sp và size nếu là pizza
            Debug.WriteLine(sp.SoLuongTon);
            if (spCheck != null)
            {
                //kiểm tra số lượng tồn trước khi cho khách hàng mua hàng
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return View("ThongBao");
                }
                spCheck.SoLuong += soLuong; // custom số lượng > 1
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                return Redirect(strURL);
            }


            itemGioHang itemGH = new itemGioHang(MaSP);
            itemGH.SoLuong = soLuong; // custom số lượng
            itemGH.Is12 = is12; // size pizza
            itemGH.TenSP = is12 ? itemGH.TenSP + " 12-inch" : itemGH.TenSP; // đổi tên ở cart nếu là pizza size 12
            itemGH.DonGia = is12 ? 149000 : itemGH.DonGia; // đổi giá ở cart nếu là pizza size 12
            itemGH.ThanhTien = itemGH.SoLuong * itemGH.DonGia; // update tổng tiền

            Debug.WriteLine(itemGH.MaSP);
            Debug.WriteLine(itemGH.ids1());
            if (sp.SoLuongTon < itemGH.SoLuong)
            {
                return View("ThongBao");
            }
            lstGioHang.Add(itemGH);
            return Redirect(strURL);
        }
        //tính tổng số lượng
        public double TinhTongSoLuong()
        {
            //lấy giỏ hàng
            List<itemGioHang> lstGioHang = Session["GioHang"] as List<itemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.SoLuong);
        }
        // tính tổng tiền
        public decimal TinhTongTien()
        {
            //lấy giỏ hàng
            List<itemGioHang> lstGioHang = Session["GioHang"] as List<itemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.ThanhTien);
        }
        public ActionResult GioHangPartial()
        {
            if (TinhTongSoLuong() == 0)
            {
                return PartialView();
            }
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }


        // GET: GioHang
        public ActionResult XemGioHang()
        {
            //lấy giỏ hàng
            List<itemGioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }
        //chỉnh sữa giỏ hàng
        [HttpGet]
        public ActionResult SuaGioHang(int MaSP)
        {
            //kiểm tra session giỏ hàng tồn tại chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //kiểm tra  sản phẩm  có tồn tại trong cơ sở dữ liệu  hay không
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //trang đường dẫn không hợp lệ
                Response.StatusCode = 404;
                return null;
            }
            //lấy list giỏ hàng từ session
            List<itemGioHang> lstGioHang = LayGioHang();
            //kiểm tra xem sản phẩm có tồn tại trong giỏ hàng hay không
            itemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");

            }
            //lấy list giỏ hàng tạo giao diện
            ViewBag.GioHang = lstGioHang;
            //nếu tồn tại rồi
            return View(spCheck);

        }
        //xử lý cập nhật giỏ hàng
        [HttpPost]
        public ActionResult CapNhatGioHang(int MaSP, int SoLuong)
        {
            //kiểm tra số lượng tồn
            SanPham spCheck = db.SanPhams.Single(n => n.MaSP == MaSP);
            if (spCheck.SoLuongTon < SoLuong)
            {
                return View("ThongBao");
            }
            //cập nhật số lượng trong session giỏ hàng
            // bước 1 : lấy list<GioHang> từ session ["GioHang"]
            List<itemGioHang> lstGH = LayGioHang();
            // tiếp theo lấy sản phẩm cập nhật từ trong list<GioHang> ra
            itemGioHang itemGHUpdate = lstGH.Find(n => n.MaSP == MaSP);
            //tiếp theo là cập nhật lại số lượng và thành tiền
            itemGHUpdate.SoLuong = SoLuong;
            itemGHUpdate.ThanhTien = itemGHUpdate.SoLuong * itemGHUpdate.DonGia;
            return RedirectToAction("XemGioHang");
        }

        public ActionResult XoaGioHang(int MaSP)
        {
            //kiểm tra session giỏ hàng tồn tại chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //kiểm tra  sản phẩm  có tồn tại trong cơ sở dữ liệu  hay không
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //trang đường dẫn không hợp lệ
                Response.StatusCode = 404;
                return null;
            }
            //lấy list giỏ hàng từ session
            List<itemGioHang> lstGioHang = LayGioHang();
            //kiểm tra xem sản phẩm có tồn tại trong giỏ hàng hay không
            itemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");

            }
            //xóa item giỏ hàng
            lstGioHang.Remove(spCheck);
            return RedirectToAction("XemGioHang");
        }
        //xây dựng chức năng đặt hàng 





        public ActionResult DatHang(KhachHang kh)
        {
            //kiểm tra session giỏ hàng tồn tại chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // thêm đơn hàng 
            DonDatHang ddh = new DonDatHang();
            ddh.NgayDat = DateTime.Now;
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            ddh.UuDai = 0;
            db.DonDatHangs.Add(ddh);
            //savechange để phát sinh MaDDH
            db.SaveChanges();
            //them chi tiết đơn đặt hàng
            List<itemGioHang> lstGH = LayGioHang();
            foreach(var item in lstGH)
            {
                ChiTietDonDatHang ctdh = new ChiTietDonDatHang();
                ctdh.MaDDH = ddh.MaDDH;
                ctdh.MaSP = item.MaSP;
                ctdh.TenSP = item.TenSP;
                ctdh.SoLuong = item.SoLuong;
                ctdh.DonGia = (int?)item.DonGia;
                db.ChiTietDonDatHangs.Add(ctdh);
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHang");

           
        }
    }
}

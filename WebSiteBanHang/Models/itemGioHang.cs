using System;
using System.Linq;

namespace WebSiteBanHang.Models
{
    public class itemGioHang
    {
        public int? MaSP { get; set; }
        public string TenSP { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
        public string HinhAnh { get; set; }
        public bool Is12 { get; set; }
        public itemGioHang(int? iMaSP)
        {
            using (QuanLyBanHangEntities db = new QuanLyBanHangEntities())
            {
                this.MaSP = iMaSP;
                SanPham sp = db.SanPhams.Single(n => n.MaSP == iMaSP);
                this.TenSP = sp.TenSP;
                this.HinhAnh = sp.HinhAnh;
                this.DonGia = sp.DonGia.Value;
                this.SoLuong = 1;
                this.ThanhTien = DonGia * SoLuong;
                this.Is12 = false;
            }
        }

        public itemGioHang(int? iMaSP, int sl)
        {
            using (QuanLyBanHangEntities db = new QuanLyBanHangEntities())
            {
                this.MaSP = iMaSP;
                SanPham sp = db.SanPhams.Single(n => n.MaSP == iMaSP);
                this.TenSP = sp.TenSP;
                this.HinhAnh = sp.HinhAnh;
                this.DonGia = sp.DonGia.Value;
                this.SoLuong = sl;
                this.ThanhTien = DonGia * SoLuong;
                this.Is12 = false;
            }
        }

        public string ids1()
        {
            String param1;
            String param2;
            String param3;
            if (this.TenSP == null) param1 = "illegal";
            else param1 = this.TenSP;
            if (this.HinhAnh == null) param2 = "illegal";
            else param2 = this.HinhAnh;
            if (this.DonGia == null) param3 = "illegal";
            else param3 = this.DonGia.ToString();
            return param1 + " " + param2 + " " + param3;
        }
    }
}

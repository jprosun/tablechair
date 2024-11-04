CREATE TABLE KichThuoc (
    MaKichThuoc NVARCHAR(20) PRIMARY KEY,
    TenKichThuoc NVARCHAR(255)
);

CREATE TABLE Loai (
    MaLoai NVARCHAR(20) PRIMARY KEY,
    TenLoai NVARCHAR(255)
);

CREATE TABLE HinhDang (
    MaHinhDang NVARCHAR(20) PRIMARY KEY,
    TenHinhDang NVARCHAR(255)
);

CREATE TABLE ChatLieu (
    MaChatLieu NVARCHAR(20) PRIMARY KEY,
    TenChatLieu NVARCHAR(255)
);

CREATE TABLE NuocSX (
    MaNuocSX NVARCHAR(20) PRIMARY KEY,
    TenNuocSX NVARCHAR(255)
);

CREATE TABLE MauSac (
    MaMau NVARCHAR(20) PRIMARY KEY,
    TenMau NVARCHAR(255)
);

CREATE TABLE DacDiem (
    MaDacDiem NVARCHAR(20) PRIMARY KEY,
    TenDacDiem NVARCHAR(255)
);

CREATE TABLE CongViec (
    MaCV NVARCHAR(20) PRIMARY KEY,
    TenCV NVARCHAR(255),
    MucLuong DECIMAL
);

CREATE TABLE NhaCungCap (
    MaNCC NVARCHAR(20) PRIMARY KEY,
    TenNCC NVARCHAR(255),
    DiaChi NVARCHAR(255),
    DienThoai NVARCHAR(20)
);

CREATE TABLE KhachHang (
    MaKhach NVARCHAR(20) PRIMARY KEY,
    TenKhach NVARCHAR(255),
    DiaChi NVARCHAR(255),
    DienThoai NVARCHAR(20)
);

CREATE TABLE NhanVien (
    MaNV NVARCHAR(20) PRIMARY KEY,
    TenNV NVARCHAR(255),
    GioiTinh NVARCHAR(10),
    NgaySinh DATE,
    DienThoai NVARCHAR(20),
    MaCV NVARCHAR(20),
    FOREIGN KEY (MaCV) REFERENCES CongViec(MaCV)
);

CREATE TABLE DMHangHoa (
    MaHang NVARCHAR(20) PRIMARY KEY,
    TenHangHoa NVARCHAR(255),
    MaLoai NVARCHAR(20),
    MaKichThuoc NVARCHAR(20),
    MaHinhDang NVARCHAR(20),
    MaChatLieu NVARCHAR(20),
    MaNuocSX NVARCHAR(20),
    CongDung NVARCHAR(255),
    MaDacDiem NVARCHAR(20),
    MaMau NVARCHAR(20),
    SoLuong INT,
    DonGiaNhap DECIMAL,
    DonGiaBan DECIMAL,
    ThoiGianBaoHanh INT,
    Anh NVARCHAR(255),
    GhiChu NVARCHAR(MAX),
    FOREIGN KEY (MaLoai) REFERENCES Loai(MaLoai),
    FOREIGN KEY (MaKichThuoc) REFERENCES KichThuoc(MaKichThuoc),
    FOREIGN KEY (MaHinhDang) REFERENCES HinhDang(MaHinhDang),
    FOREIGN KEY (MaChatLieu) REFERENCES ChatLieu(MaChatLieu),
    FOREIGN KEY (MaNuocSX) REFERENCES NuocSX(MaNuocSX),
    FOREIGN KEY (MaDacDiem) REFERENCES DacDiem(MaDacDiem),
    FOREIGN KEY (MaMau) REFERENCES MauSac(MaMau)
);

CREATE TABLE HoaDonBan (
    SoHDB NVARCHAR(20) PRIMARY KEY,
    MaNV NVARCHAR(20),
    NgayBan DATE,
    MaKhach NVARCHAR(20),
    TongTien DECIMAL,
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV),
    FOREIGN KEY (MaKhach) REFERENCES KhachHang(MaKhach)
);

CREATE TABLE ChiTietHoaDonBan (
    SoHDB NVARCHAR(20),
    MaHang NVARCHAR(20),
    SoLuong INT,
    GiamGia DECIMAL,
    ThanhTien DECIMAL,
    PRIMARY KEY (SoHDB, MaHang),
    FOREIGN KEY (SoHDB) REFERENCES HoaDonBan(SoHDB),
    FOREIGN KEY (MaHang) REFERENCES DMHangHoa(MaHang)
);

CREATE TABLE HoaDonNhap (
    SoHDN NVARCHAR(20) PRIMARY KEY,
    MaNV NVARCHAR(20),
    NgayNhap DATE,
    MaNCC NVARCHAR(20),
    TongTien DECIMAL,
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV),
    FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC)
);

CREATE TABLE ChiTietHoaDonNhap (
    SoHDN NVARCHAR(20),
    MaHang NVARCHAR(20),
    SoLuong INT,
    DonGia DECIMAL,
    GiamGia DECIMAL,
    ThanhTien DECIMAL,
    PRIMARY KEY (SoHDN, MaHang),
    FOREIGN KEY (SoHDN) REFERENCES HoaDonNhap(SoHDN),
    FOREIGN KEY (MaHang) REFERENCES DMHangHoa(MaHang)
);

CREATE TABLE login (
    email NVARCHAR(255) PRIMARY KEY,
    password NVARCHAR(255) NOT NULL
);


INSERT INTO login (email, password) 
VALUES ('admin', '123');



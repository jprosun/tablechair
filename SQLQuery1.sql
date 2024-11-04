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




-- Insert dữ liệu cho bảng KichThuoc
INSERT INTO KichThuoc (MaKichThuoc, TenKichThuoc) 
VALUES 
('KT001', N'Nhỏ'),
('KT002', N'Vừa'),
('KT003', N'Lớn');

-- Insert dữ liệu cho bảng Loai
INSERT INTO Loai (MaLoai, TenLoai) 
VALUES 
('L001', N'Bàn'),

('L002', N'Ghế');
INSERT INTO Loai (MaLoai, TenLoai) 
VALUES 

('L003', N'bong');


-- Insert dữ liệu cho bảng HinhDang
INSERT INTO HinhDang (MaHinhDang, TenHinhDang) 
VALUES 
('HD001', N'Hình Tròn'),
('HD002', N'Hình Vuông'),
('HD003', N'Hình Chữ Nhật');

-- Insert dữ liệu cho bảng ChatLieu
INSERT INTO ChatLieu (MaChatLieu, TenChatLieu) 
VALUES 
('CL001', N'Gỗ'),
('CL002', N'Nhựa'),
('CL003', N'Kim Loại');

INSERT INTO ChatLieu (MaChatLieu, TenChatLieu) 
VALUES 
('CL006', N'đá');

-- Insert dữ liệu cho bảng NuocSX
INSERT INTO NuocSX (MaNuocSX, TenNuocSX) 
VALUES 
('NSX001', N'Việt Nam'),
('NSX002', N'Trung Quốc'),
('NSX003', N'Mỹ');

-- Insert dữ liệu cho bảng MauSac
INSERT INTO MauSac (MaMau, TenMau) 
VALUES 
('M001', N'Đỏ'),
('M002', N'Xanh Dương'),
('M003', N'Vàng');

-- Insert dữ liệu cho bảng DacDiem
INSERT INTO DacDiem (MaDacDiem, TenDacDiem) 
VALUES 
('DD001', N'Chống Nước'),
('DD002', N'Chống Trầy Xước'),
('DD003', N'Chống Ẩm');

-- Insert dữ liệu cho bảng CongViec
INSERT INTO CongViec (MaCV, TenCV, MucLuong) 
VALUES 
('CV001', N'Nhân Viên Bán Hàng', 7000000),
('CV002', N'Quản Lý', 12000000);

-- Insert dữ liệu cho bảng NhaCungCap
INSERT INTO NhaCungCap (MaNCC, TenNCC, DiaChi, DienThoai) 
VALUES 
('NCC001', N'Công Ty A', N'Hà Nội', '0123456789'),
('NCC002', N'Công Ty B', N'Hồ Chí Minh', '0987654321');

-- Insert dữ liệu cho bảng KhachHang
INSERT INTO KhachHang (MaKhach, TenKhach, DiaChi, DienThoai) 
VALUES 
('KH001', N'Nguyễn Văn A', N'Hà Nội', '0123456789'),
('KH002', N'Hoàng Thị B', N'Hồ Chí Minh', '0987654321');

-- Insert dữ liệu cho bảng NhanVien
INSERT INTO NhanVien (MaNV, TenNV, GioiTinh, NgaySinh, DienThoai, MaCV) 
VALUES 
('NV001', N'Trần Văn C', N'Nam', '1990-05-10', '0123456789', 'CV001'),
('NV002', N'Lê Thị D', N'Nữ', '1985-03-22', '0987654321', 'CV002');

-- Insert dữ liệu cho bảng DMHangHoa
INSERT INTO DMHangHoa (MaHang, TenHangHoa, MaLoai, MaKichThuoc, MaHinhDang, MaChatLieu, MaNuocSX, CongDung, MaDacDiem, MaMau, SoLuong, DonGiaNhap, DonGiaBan, ThoiGianBaoHanh, Anh, GhiChu) 
VALUES 
('HH001', N'Bàn Tròn Gỗ', 'L001', 'KT002', 'HD001', 'CL001', 'NSX001', N'Phù hợp cho phòng họp', 'DD002', 'M001', 10, 500000, 700000, 24, NULL, N'Không có ghi chú'),
('HH002', N'Ghế Nhựa Xanh', 'L002', 'KT001', 'HD002', 'CL002', 'NSX002', N'Phù hợp cho không gian ngoài trời', 'DD001', 'M002', 20, 200000, 350000, 12, NULL, N'Không có ghi chú');

-- Insert dữ liệu cho bảng HoaDonBan
INSERT INTO HoaDonBan (SoHDB, MaNV, NgayBan, MaKhach, TongTien) 
VALUES 
('HDB001', 'NV001', '2024-10-23', 'KH001', 700000),
('HDB002', 'NV002', '2024-10-23', 'KH002', 350000);

-- Insert dữ liệu cho bảng ChiTietHoaDonBan
INSERT INTO ChiTietHoaDonBan (SoHDB, MaHang, SoLuong, GiamGia, ThanhTien) 
VALUES 
('HDB001', 'HH001', 1, 0, 700000),
('HDB002', 'HH002', 1, 0, 350000);

-- Insert dữ liệu cho bảng HoaDonNhap
INSERT INTO HoaDonNhap (SoHDN, MaNV, NgayNhap, MaNCC, TongTien) 
VALUES 
('HDN001', 'NV001', '2024-10-22', 'NCC001', 1000000),
('HDN002', 'NV002', '2024-10-22', 'NCC002', 700000);

-- Insert dữ liệu cho bảng ChiTietHoaDonNhap
INSERT INTO ChiTietHoaDonNhap (SoHDN, MaHang, SoLuong, DonGia, GiamGia, ThanhTien) 
VALUES 
('HDN001', 'HH001', 10, 500000, 0, 5000000),
('HDN002', 'HH002', 20, 200000, 0, 4000000);



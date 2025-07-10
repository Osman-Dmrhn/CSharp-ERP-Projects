# 🌐 ProductionAndStockERP

**ProductionAndStockERP**, üretim ve stok yönetimini kolaylaştıran bir kurumsal kaynak planlaması (ERP) sistemidir. Kullanıcıların üretim süreçlerini, stok hareketlerini ve siparişlerini verimli bir şekilde yönetmelerini sağlar.

**Not**: Bu proje şu an yapım aşamasındadır ve tüm özellikler tamamen işlevsel olmayabilir. Özellikler ve fonksiyonlar kademeli olarak eklenmektedir.

## Özellikler
- **Sipariş Yönetimi**: Siparişleri oluşturma, güncelleme ve takip etme.
- **Üretim Yönetimi**: Üretim emirlerini oluşturma ve stok güncellemeleri yapma.
- **Stok Yönetimi**: Stok seviyelerini izleme ve yönetme.
- **JWT Kimlik Doğrulama**: Kullanıcılar güvenli bir şekilde sisteme giriş yapar ve yetkili işlemleri gerçekleştirebilir.
- **Aktivite Kayıtları**: Kullanıcı eylemleri takip edilir ve kaydedilir.

## Teknolojiler
- **Backend**: ASP.NET Core Web API
- **Veritabanı**: SQL Server (Entity Framework üzerinden)
- **Kimlik Doğrulama**: JWT

## Temel Bileşenler
- **Sipariş Yönetimi**: Sipariş oluşturma, güncelleme ve takip etme işlemleri.
- **Üretim Emirleri**: Üretim emirleri ve üretim süreçlerini yönetme.
- **Stok Hareketleri**: Ürün giriş ve çıkışlarını yönetme.
- **Kullanıcı Yönetimi**: Kullanıcılar, roller ve yetkilendirme işlemleri.
- **Aktivite Logları**: Sistemdeki tüm kullanıcı aktivitelerini kaydeder.

## Kullanım
- **JWT Kimlik Doğrulama**: Kullanıcılar sisteme giriş yaparak üretim ve stok yönetim işlemlerine erişim sağlar.
- **REST API**: API üzerinden siparişler, üretim emirleri, stok işlemleri ve kullanıcı aktiviteleri ile ilgili veri alınıp gönderilebilir.

Bu proje, işletmelere üretim süreçlerini ve stoklarını dijital ortamda yönetme imkanı sağlar, böylece verimliliği artırır ve operasyonel hataları azaltır.

---

# 🌐 ProductionAndStockERP

**ProductionAndStockERP** is an enterprise resource planning (ERP) system designed to simplify production and stock management. It allows users to efficiently manage production processes, stock movements, and orders.

**Note**: This project is currently under development and may not be fully functional. Features and functionalities are being added progressively.

## Features
- **Order Management**: Create, update, and track orders.
- **Production Management**: Create production orders and update stock levels.
- **Stock Management**: Monitor and manage stock levels.
- **JWT Authentication**: Secure login for users and authorized operations.
- **Activity Logs**: Track and record user actions.

## Technologies
- **Backend**: ASP.NET Core Web API
- **Database**: SQL Server (via Entity Framework)
- **Authentication**: JWT
- **UI**: Modern and responsive design

## Core Components
- **Order Management**: Creating, updating, and tracking orders.
- **Production Orders**: Managing production orders and production processes.
- **Stock Movements**: Managing product entries and exits.
- **User Management**: Users, roles, and authorization.
- **Activity Logs**: Recording all user activities within the system.

## Usage
- **JWT Authentication**: Users can log in and access production and stock management operations.
- **REST API**: API endpoints allow data related to orders, production orders, stock movements, and user activities to be retrieved and sent.

This project enables businesses to manage their production processes and stocks in a digital environment, improving efficiency and reducing operational errors.

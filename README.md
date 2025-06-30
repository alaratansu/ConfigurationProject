# 🛠 Configuration Management System

Bu proje, uygulamalar için merkezi ve dinamik bir konfigürasyon yönetim sistemi sunar. 

## 📌 Özellikler

- Her uygulama kendi konfigürasyonlarını izole şekilde yönetebilir.
- Konfigürasyonlara `ConfigurationReader` sınıfı ile runtime'da erişilir.
- `RefreshTimer` sayesinde belirli aralıklarla konfigürasyonlar güncellenir.
- `Redis` ile fallback cache mekanizması mevcuttur.
- Konfigürasyon güncellemeleri `RabbitMQ` üzerinden diğer instance'lara yayınlanır.
- Swagger arayüzü üzerinden CRUD işlemleri yapılabilir.
- Docker Compose desteği ile hızlı kurulum.

## 🧱 Katmanlar

- `Configuration.Api` – REST API (CRUD işlemleri)
- `Configuration.Application` – CQRS & iş kuralları
- `Configuration.Infrastructure` – Redis & RabbitMQ servisleri
- `Configuration.Persistence` – EF Core, DbContext, migrasyonlar
- `Configuration.Library` – Dış projelerde kullanılabilecek `ConfigurationReader`
- `Configuration.Tests` – xUnit testleri (Unit & Integration)

## 📦 Kullanılan Teknolojiler

- .NET 5
- Entity Framework Core
- Redis
- RabbitMQ
- MediatR (CQRS)
- FluentValidation
- xUnit
- Docker & Docker Compose
- AutoMapper
- Swagger

## 🚀 Kurulum

1. **Docker Compose ile başlat:**
   ```bash
   docker-compose up --build
   
---

### 👤 Geliştirici

**Alara Akgün** 

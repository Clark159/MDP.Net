---
layout: default
title: 服務注入-DecoratorPattern
parent: 依賴注入(Hosting)
nav_order: 6
---

# 服務注入-DecoratorPattern

在MDP.Net核心模組中，「服務注入模組」注入使用ServiceAttribute註冊的Instance時，可以在建構子注入另外一個Instance，進行逐層的服務注入。依照下列的類別宣告及參數設定，就可以在系統裡註冊Type(DecorateDemoService)為Service(DemoService)的Instance，並且逐層注入Service(DemoService)。

```csharp
using MDP.Registration;
using System;

namespace WebApplication1
{
    [Service<DemoService>()]
    public class DecorateDemoService : DemoService
    {
        // Constructors
        public DecorateDemoService(DemoService component)
        {
            //...
        }
    }
}
```

```json
{
  "WebApplication1": {

    "DecorateDemoService": {
      "component": "DecorateDemoService[A]"
    },
    
    "DecorateDemoService[A]": {
      "component": "DecorateDemoService[B]"
    },
    
    "DecorateDemoService[B]": {
      "component": "DecorateDemoService[C]"
    },
    
    //...
  }
}
```

本篇文件介紹，如何使用MDP.Net核心模組中「服務注入模組」，注入使用ServiceAttribute註冊的Instance時，在建構子注入另外一個Instance，進行逐層的服務注入。

## 操作步驟

### 1. 建立新專案

依照「[建立MvcPage專案](../../QuickStart/建立MvcPage專案/建立MvcPage專案.html)」的操作步驟，建立新的MvcPage專案「WebApplication1」。

### 2. 新增DemoService

在專案裡新增Modules資料夾，加入DemoService.cs。

```csharp
using MDP.Registration;

namespace WebApplication1
{
    public interface DemoService
    {
        // Methods
        string GetMessage();
    }
}
```

### 2. 新增DecorateDemoService

在專案裡的Modules資料夾，加入DecorateDemoService.cs。除使用ServiceAttribute註冊DecorateDemoService為DemoService的Instance，由建構子接受string message, DemoService component兩個參數，接收逐層注入的DemoService。

```csharp
using MDP.Registration;
using System;

namespace WebApplication1
{
    [Service<DemoService>()]
    public class DecorateDemoService : DemoService
    {
        // Fields
        private readonly string _message;

        private readonly DemoService _component ;


        // Constructors
        public DecorateDemoService(string message, DemoService component)
        {
            // Default
            _message = message;
            _component  = component ;
        }


        // Methods
        public string GetMessage()
        {
            // Return
            return _message + "-->" + _component.GetMessage();
        }
    }
}
```

### 3. 新增ConcreteDemoService

在專案裡的Modules資料夾，加入ConcreteDemoService.cs。同樣使用ServiceAttribute註冊DecorateDemoService為DemoService的Instance，但是建構子只接受string message參數，做為逐層注入DemoService的最後一個注入。

```csharp
using MDP.Registration;

namespace WebApplication1
{
    [Service<DemoService>()]
    public class ConcreteDemoService : DemoService
    {
        // Fields
        private readonly string _message;


        // Constructors
        public ConcreteDemoService(string message)
        {
            // Default
            _message = message;
        }


        // Methods
        public string GetMessage()
        {
            // Return
            return _message;
        }
    }
}
```

### 4. 修改appsettings.json

在專案裡修改appsettings.json，加入逐層注入DemoService的註冊參數設定。

```json
{
  "WebApplication1": {

    // Typed
    "DecorateDemoService": {
      "Message": "Decorator2",
      "component": "DecorateDemoService[A]"
    },

    // Decorate
    "DecorateDemoService[A]": {
      "Message": "Decorator1",
      "component": "ConcreteDemoService[B]"
    },

    // Concrete
    "ConcreteDemoService[B]": {
      "Message": "Hello World"
    }
  }
}
```

### 5. 修改HomeController

在專案裡修改HomeController.cs，注入DemoService的TypedInstance。

```csharp
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1
{
    public class HomeController : Controller
    {
        // Fields
        private readonly DemoService _demoService;


        // Constructors
        public HomeController(DemoService demoService)
        {
            // Default
            _demoService = demoService;
        }


        // Methods
        public ActionResult Index()
        {
            // Message
            this.ViewBag.Message = _demoService.GetMessage();

            // Return
            return View();
        }
    }
}

```

### 6. 執行專案

完成以上操作步驟後，就已成功在MvcPage專案中使用服務注入-DecoratorPattern。按F5執行專案，使用Browser開啟Page：/Home/Index，可以在網頁內容看到下列，由層層服務注入的DemoService所提供的訊息內容。

```
Decorator2-->Decorator1-->Hello World
```

## 範例檔案

[https://github.com/Clark159/MDP.Net/tree/master/demo/Hosting/服務注入-DecoratorPattern](https://github.com/Clark159/MDP.Net/tree/master/demo/Hosting/服務注入-DecoratorPattern)
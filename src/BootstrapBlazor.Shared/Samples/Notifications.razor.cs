﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using BootstrapBlazor.Shared.Common;
using BootstrapBlazor.Shared.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

namespace BootstrapBlazor.Shared.Samples;

/// <summary>
/// Notifications 通知
/// </summary>
public partial class Notifications : IDisposable
{
    private JSInterop<Notifications>? Interop { get; set; }

    [NotNull]
    private BlockLogger? Trace { get; set; }

    [Inject]
    [NotNull]
    private IStringLocalizer<Notifications>? Localizer { get; set; }

    [Inject]
    [NotNull]
    private IJSRuntime? JSRuntime { get; set; }

    [Inject]
    [NotNull]
    private NotificationService? NotificationService { get; set; }

    private string result { get; set; }
    private bool permission { get; set; }
    private NotificationItem Model { get; set; } = new NotificationItem();

    /// <summary>
    /// OnInitialized 方法
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        Model.Title ??= "你有新的物流通知";
        Model.Message ??= "您的包裹已到达亮马河南路,距离分发只剩3站.";
        Model.Icon ??= "_content/BootstrapBlazor.Shared/images/Argo-C.png";
        //Model.OnClick ??= nameof(OnClickNotificationCallback);
    }

    /// <summary>
    /// OnAfterRenderAsync 方法
    /// </summary>
    /// <param name="firstRender"></param>
    /// <returns></returns>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Interop = new JSInterop<Notifications>(JSRuntime);
            var ret = await NotificationService.CheckPermission(Interop, this, nameof(GetPermissionCallback), false);
            Trace.Log(ret ? Localizer["CheckPermissionResultSuccess"] : Localizer["CheckPermissionResultFailed"]);
        }
    }

    private async Task CheckPermission()
    {
        Interop ??= new JSInterop<Notifications>(JSRuntime);
        var ret = await NotificationService.CheckPermission(Interop, this, nameof(GetPermissionCallback));
        Trace.Log(ret ? Localizer["CheckPermissionResultSuccess"] : Localizer["CheckPermissionResultFailed"]);
    }

    private async Task DisplayNotification()
    {
        Interop ??= new JSInterop<Notifications>(JSRuntime);
        var ret = await NotificationService.DisplayNotification(Interop, this, nameof(ShowNotificationCallback), Model);
        Trace.Log(ret ? Localizer["NotificationResultSuccess"] : Localizer["NotificationResultFailed"]);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="result"></param>
    [JSInvokable]
    public void GetPermissionCallback(string result)
    {
        this.permission = result == "true";
        Trace.Log("GetPermissionCallback " + (this.permission ? "OK": "No permission"));
        StateHasChanged();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="result"></param>
    [JSInvokable]
    public void ShowNotificationCallback(string result)
    {
        this.result = result;
        Trace.Log("ShowNotificationCallback " + result);
        StateHasChanged();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="result"></param>
    [JSInvokable]
    public void OnClickNotificationCallback(string result)
    {
        this.result = result;
        Trace.Log("OnClickNotificationCallback " + result);
        StateHasChanged();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (Interop != null)
            {
                Interop.Dispose();
                Interop = null;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private IEnumerable<AttributeItem> GetNotificationItem() => new AttributeItem[]
    {
        new() {
            Name = "Title",
            Description = Localizer["TitleText"],
            Type = "string",
            ValueList = "-",
            DefaultValue = ""
        },
        new() {
            Name = "Message",
            Description = Localizer["MessageText"],
            Type = "string",
            ValueList = "-",
            DefaultValue = ""
        },
        new() {
            Name = "Icon",
            Description = Localizer["IconText"],
            Type = "string",
            ValueList = "-",
            DefaultValue = ""
        },
        new() {
            Name = "Silent",
            Description = Localizer["SilentText"],
            Type = "bool",
            ValueList = "-",
            DefaultValue = ""
        },
        new() {
            Name = "Sound",
            Description = Localizer["SoundText"],
            Type = "string",
            ValueList = "-",
            DefaultValue = ""
        },
        new() {
            Name = "OnClick",
            Description = Localizer["OnClickText"],
            Type = "Methods",
            ValueList = "-",
            DefaultValue = ""
        },
    };

    private IEnumerable<MethodItem> GetMethods() => new MethodItem[]
    {
        new()
        {
            Name ="NotificationService." + nameof(NotificationService.CheckPermission),
            Description = Localizer["CheckPermissionText"],
            Parameters = "JSRuntime实例,TComponent,检查通知权限结果回调方法,是否弹出获取权限窗口",
            ReturnValue = "bool"
        },
        new()
        {
            Name ="NotificationService." + nameof(NotificationService.DisplayNotification),
            Description = Localizer["NotificationButtonText"],
            Parameters = "JSRuntime实例,TComponent,发送结果回调方法",
            ReturnValue = "object"
        }, 
    };

}
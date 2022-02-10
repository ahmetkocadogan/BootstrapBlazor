// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

namespace BootstrapBlazor.Components;

/// <summary>
/// 浏览器通知服务
/// </summary>
public class NotificationService
{

    /// <summary>
    /// 检查浏览器通知权限状态
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    /// <param name="interop"></param>
    /// <param name="component"></param>
    /// <param name="callbackMethodName">检查通知权限结果回调方法</param>
    /// <param name="requestPermission"></param>
    /// <returns></returns>
    public async Task<bool> CheckPermission<TComponent>(JSInterop<TComponent> interop, TComponent component, string callbackMethodName, bool requestPermission = true) where TComponent : class
    {
        var ret = await interop.CheckNotifyPermissionAsync(component, callbackMethodName, requestPermission);
        return ret;
    }

    /// <summary>
    /// 发送浏览器通知
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    /// <param name="interop"></param>
    /// <param name="component"></param>
    /// <param name="callbackMethodName">发送结果回调方法</param>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> DisplayNotification<TComponent>(JSInterop<TComponent> interop, TComponent component, string callbackMethodName, NotificationItem model) where TComponent : class
    {
        var ret = await interop.DisplayNotification(component, callbackMethodName, model);
        return ret;
    }
}

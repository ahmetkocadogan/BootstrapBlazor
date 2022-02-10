// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Components;

public class SwalTest : BootstrapBlazorTestBase
{
    [Fact]
    public void Show_Ok()
    {
        var cut = Context.RenderComponent<BootstrapBlazorRoot>(pb =>
        {
            pb.AddChildContent<MockSwalTest>();
        });

        var swal = cut.FindComponent<MockSwalTest>().Instance.SwalService;

        cut.InvokeAsync(() => swal.Show(new SwalOption()
        {
            BodyTemplate = builder => builder.AddContent(0, "Test-BodyTemplate"),
            FooterTemplate = builder => builder.AddContent(0, "Test-FooterTemplate"),
            ButtonTemplate = builder => builder.AddContent(0, "Test-ButtonTemplate"),
            ShowFooter = true,
        }));

        // 代码覆盖模板单元测试
        Assert.Contains("Test-BodyTemplate", cut.Markup);
        Assert.Contains("Test-FooterTemplate", cut.Markup);
        Assert.Contains("Test-ButtonTemplate", cut.Markup);

        var aa = cut.Markup;

        // 测试关闭逻辑
        var modal = cut.FindComponent<Modal>();
        cut.InvokeAsync(() => modal.Instance.Close());

        //测试Content
        cut.InvokeAsync(() => swal.Show(new SwalOption()
        {
            Content = "I am Swal",
        }));

        Assert.Contains("I am Swal", cut.Markup);

        modal = cut.FindComponent<Modal>();
        cut.InvokeAsync(() => modal.Instance.Close());

        //测试Title
        cut.InvokeAsync(() => swal.Show(new SwalOption()
        {
            Content = "I am Title",
        }));

        Assert.Contains("I am Title", cut.Markup);

        modal = cut.FindComponent<Modal>();
        cut.InvokeAsync(() => modal.Instance.Close());

        //测试Title
        cut.InvokeAsync(() => swal.Show(new SwalOption()
        {
            ForceDelay = true,
            Delay = 1000
        }));

        modal = cut.FindComponent<Modal>();
        cut.InvokeAsync(() => modal.Instance.Close());

        //测试Title
        cut.InvokeAsync(() => swal.Show(new SwalOption()
        {
            ForceDelay = true,
            Delay = 1000,
        }));

        modal = cut.FindComponent<Modal>();
        cut.InvokeAsync(() => modal.Instance.Close());

        //测试关闭按钮
        cut.InvokeAsync(() => swal.Show(new SwalOption()
        {
            Content = "I am Swal",
        }));

        var button = cut.Find(".btn-secondary");
        button.Click();

        //测试Modal取消
        var cancel = true;
        cut.InvokeAsync(async () =>
        {
            cancel = await swal.ShowModal(new SwalOption()
            {
                Content = "I am Swal",
                IsConfirm = true
            });
        });

        var cancelbutton = cut.Find(".btn-secondary");
        cancelbutton.Click();
        Assert.False(cancel);

        //测试Modal确认
        var confirm = false;
        cut.InvokeAsync(async () =>
        {
            confirm = await swal.ShowModal(new SwalOption()
            {
                Content = "I am Swal",
                IsConfirm = true,
            });
        });

        var confirmbutton = cut.Find(".btn-danger");
        confirmbutton.Click();
        Assert.True(confirm);
    }

    private class MockSwalTest : ComponentBase
    {
        [Inject]
        [NotNull]
        public SwalService? SwalService { get; set; }
    }
}



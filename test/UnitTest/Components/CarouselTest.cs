// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Shared;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using UnitTest.Extensions;

namespace UnitTest.Components;

public class CarouselTest : BootstrapBlazorTestBase
{
    [Fact]
    public void Images_Ok()
    {
        var cut = Context.RenderComponent<Carousel>(pb =>
        {
            pb.Add(b => b.Images, new List<string>() { "../Images/1.jpg", "../Images/2.jpg", "../Images/3.jpg", "../Images/4.jpg" });
        });
        Assert.Contains("../Images/1.jpg", cut.Markup);
        Assert.Contains("../Images/2.jpg", cut.Markup);
        //Assert.Contains("../Images/3.jpg", cut.Markup);
        //Assert.Contains("../Images/4.jpg", cut.Markup);
        cut.SetParametersAndRender<Carousel>(pb =>
        {
            pb.Add(b => b.Width, 100);
        });
        Assert.Contains("style=\"width: 100px;\"", cut.Markup);
        cut.SetParametersAndRender<Carousel>(pb =>
        {
            pb.Add(b => b.IsFade, true);
        });
        Assert.Contains("carousel-fade", cut.Markup);

    }

    [Fact]
    public async Task Click_Ok()
    {
        // 同步点击
        string clicked = string.Empty;
        var cut = Context.RenderComponent<Carousel>(pb =>
        {
            pb.Add(b => b.Images, new List<string>() { "../Images/1.jpg", "../Images/2.jpg", "../Images/3.jpg", "../Images/4.jpg" });
            pb.Add(b => b.OnClick, img =>
            {
                clicked = img;
                return Task.CompletedTask;
            });
        });
        var bs = cut.FindAll("img");
        for (int i = 0; i < bs.Count; i++)
        {
            var b = bs[i];
            b.Click();
            Assert.Equal($"../Images/{i + 1}.jpg", clicked);
        }

    }
}

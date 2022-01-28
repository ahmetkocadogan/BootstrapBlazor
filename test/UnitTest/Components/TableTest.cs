// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.ComponentModel;

namespace UnitTest.Components;

public class TableTest : BootstrapBlazorTestBase
{
    private IStringLocalizer<Foo> Localizer { get; }

    public TableTest()
    {
        Localizer = Context.Services.GetRequiredService<IStringLocalizer<Foo>>();
    }

    [Fact]
    public void TablesDetailRow_Ok()
    {
        var detailGenerated = false;
        var cut = Context.RenderComponent<BootstrapBlazorRoot>(pb =>
        {
            pb.AddChildContent<Table<Foo>>(pb =>
            {
                pb.Add(a => a.OnQueryAsync, OnQueryAsync);
                pb.Add(a => a.RenderMode, TableRenderMode.Table);
                pb.Add(a => a.AutoGenerateColumns, true);
                pb.Add(a => a.DetailRowTemplate, item => builder =>
                {
                    detailGenerated = true;
                    builder.OpenComponent(0, typeof(Table<DetailRow>));
                    builder.AddAttribute(1, nameof(Table<DetailRow>.Items), DetailRow.GenerateDetailRow(item.Id));
                    builder.AddAttribute(2, nameof(Table<DetailRow>.AutoGenerateColumns), true);
                    builder.CloseComponent();
                });
            });
        });
        var table = cut.FindComponent<Table<Foo>>();
        var arrow = table.Find(".fa-caret-right");
        arrow.Click();

        // 断言明细行已生成
        Assert.True(detailGenerated);

        Task<QueryData<Foo>> OnQueryAsync(QueryPageOptions options)
        {
            var items = Foo.GenerateFoo(Localizer, 2);
            return Task.FromResult(new QueryData<Foo>()
            {
                TotalCount = items.Count,
                Items = items
            });
        }
    }

    private class DetailRow
    {
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("主键")]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("培训课程")]
        [AutoGenerateColumn(Order = 10)]
        public string Name { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("日期")]
        [AutoGenerateColumn(Order = 20, Width = 180)]
        public DateTime DateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<DetailRow> GenerateDetailRow(int count = 3) => Enumerable.Range(1, count).Select(i => new DetailRow()
        {
            Id = i,
            Name = "DetailRow" + i.ToString(),
            DateTime = DateTime.Now.AddDays(-count)
        }).ToList();
    }
}

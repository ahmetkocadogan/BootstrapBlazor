﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UnitTest.Components;

public class TableTest : BootstrapBlazorTestBase
{
    /// <summary>
    ///
    /// </summary>
    public class Foo
    {
        // 列头信息支持 Display DisplayName 两种标签

        /// <summary>
        ///
        /// </summary>
        [Key]
        [Display(Name = "主键")]
        [AutoGenerateColumn(Ignore = true)]
        public int Id { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Required(ErrorMessage = "{0}不能为空")]
        [AutoGenerateColumn(Order = 10, Filterable = true, Searchable = true)]
        [Display(Name = "姓名")]
        public string? Name { get; set; } 
        /// <summary>
        ///
        /// </summary>
        [Required(ErrorMessage = "请选择学历")]
        [Display(Name = "学历")]
        [AutoGenerateColumn(Order = 60)]
        public EnumEducation? Education { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Foo> GenerateFoo( int count = 10) => Enumerable.Range(1, count).Select(i => new Foo()
        {
            Id = i,
            Name = "Foo" + i.ToString(),
            Education = random.Next(1, 100) > 50 ? EnumEducation.Primary : EnumEducation.Middel
        }).ToList();
        private static readonly Random random = new();

    }

    /// <summary>
    ///
    /// </summary>
    public enum EnumEducation
    {
        /// <summary>
        ///
        /// </summary>
        [Display(Name = "小学")]
        Primary,

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "中学")]
        Middel
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
            DateTime=DateTime.Now.AddDays(-count)
        }).ToList();

    }

    private List<Foo>? Items { get; set; } = Foo.GenerateFoo();
    private List<DetailRow>? DetailRows(int i) => DetailRow.GenerateDetailRow(i);
    private bool flag;
        // 异步点击
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

    [Fact]
    public async void TablesDetailRow_Ok()
    {
        var cut = Context.RenderComponent<BootstrapBlazorRoot>(pb =>
        {
            pb.AddChildContent(TablesDetailRow());
        });
        var table = cut.FindComponent<Table<Foo>>();
        await table.InvokeAsync(async () => { await table.Instance.QueryAsync(); tcs.SetResult(true);});
        await tcs.Task;
        var html = table.Markup;


        //因为懒加载 loading...无法测试 , :-<

        //var cut = Context.RenderComponent<Table<Foo>>(builder => {
        //    builder.Add(a => a.AutoGenerateColumns, true);
        //    builder.Add(a => a.Items, Items);
        //    builder.Add(a => a.IsDetails, _=true);
        //    builder.Add(a => a.OnAfterRenderCallback, OnAfterRenderCallback());
        //    builder.Add(a => a.DetailRowTemplate, GenerateDetailRow());
        //    }
        //);
        //cut.InvokeAsync(() => cut.Instance.OnAfterRenderCallback());
        Assert.Contains("class=\"form-control checkbox-list\"", cut.Markup);
    }
    public RenderFragment TablesDetailRow() => builder =>
    {
        builder.OpenComponent(0, typeof(Table<Foo>));
        builder.AddAttribute(1, nameof(Table<Foo>.OnQueryAsync),((QueryPageOptions options) => OnQueryAsync(options)));
        //builder.AddAttribute(1, nameof(Table<Foo>.Items), Items);
        builder.AddAttribute(2, nameof(Table<Foo>.IsDetails), _ = true);
        builder.AddAttribute(3, nameof(Table<Foo>.OnAfterRenderCallback), OnAfterRenderCallback());
        builder.AddAttribute(4, nameof(Table<Foo>.DetailRowTemplate), GenerateDetailRow());
        builder.AddAttribute(5, nameof(Table<Foo>.AutoGenerateColumns), true);
        builder.CloseComponent();
    };


    Func<Table<Foo>, Task>? OnAfterRenderCallback()
    {
        return  _ => Task.CompletedTask;
    }
    public Task<QueryData<Foo>> OnQueryAsync(QueryPageOptions options)
    {
        flag = !flag;
        return Task.FromResult( new QueryData<Foo>()
        {
            TotalCount = flag?0: Items?.Count??0,
            Items = flag ? null : Items
        }) ;
    }


    private RenderFragment<Foo> GenerateDetailRow() 
    {
        return  new RenderFragment<Foo>(item => builder =>
        {
            builder.OpenComponent(0, typeof(Table<DetailRow>));
            builder.AddAttribute(1, nameof(Table<DetailRow>.Items), DetailRows(item.Id));
            builder.AddAttribute(2, nameof(Table<DetailRow>.AutoGenerateColumns), true);
            builder.CloseComponent();
        }); 
    }


}

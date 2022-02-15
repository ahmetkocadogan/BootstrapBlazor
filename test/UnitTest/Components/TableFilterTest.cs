// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using UnitTest.Extensions;

namespace UnitTest.Components;

public class TableFilterTest : BootstrapBlazorTestBase
{
    private static RenderFragment<Foo> CreateTableColumns() => foo => builder =>
    {
        var index = 0;
        builder.OpenComponent<TableColumn<Foo, string>>(index++);
        builder.AddAttribute(index++, nameof(TableColumn<Foo, string>.Field), foo.Name);
        builder.AddAttribute(index++, nameof(TableColumn<Foo, string>.FieldExpression), foo.GenerateValueExpression());
        builder.AddAttribute(index++, nameof(TableColumn<Foo, string>.Filterable), true);
        builder.CloseComponent();

        builder.OpenComponent<TableColumn<Foo, bool>>(index++);
        builder.AddAttribute(index++, nameof(TableColumn<Foo, bool>.Field), foo.Complete);
        builder.AddAttribute(index++, nameof(TableColumn<Foo, bool>.FieldExpression), foo.GenerateValueExpression(nameof(Foo.Complete), typeof(bool)));
        builder.AddAttribute(index++, nameof(TableColumn<Foo, bool>.Filterable), true);
        builder.CloseComponent();

        builder.OpenComponent<TableColumn<Foo, int>>(index++);
        builder.AddAttribute(index++, nameof(TableColumn<Foo, int>.Field), foo.Count);
        builder.AddAttribute(index++, nameof(TableColumn<Foo, int>.FieldExpression), foo.GenerateValueExpression(nameof(Foo.Count), typeof(int)));
        builder.AddAttribute(index++, nameof(TableColumn<Foo, int>.Filterable), true);
        builder.CloseComponent();

        builder.OpenComponent<TableColumn<Foo, EnumEducation?>>(index++);
        builder.AddAttribute(index++, nameof(TableColumn<Foo, EnumEducation?>.Field), foo.Education);
        builder.AddAttribute(index++, nameof(TableColumn<Foo, EnumEducation?>.FieldExpression), foo.GenerateValueExpression(nameof(Foo.Education), typeof(EnumEducation?)));
        builder.AddAttribute(index++, nameof(TableColumn<Foo, EnumEducation?>.Filterable), true);
        builder.CloseComponent();

        builder.OpenComponent<TableColumn<Foo, DateTime?>>(index++);
        builder.AddAttribute(index++, nameof(TableColumn<Foo, DateTime?>.Field), foo.DateTime);
        builder.AddAttribute(index++, nameof(TableColumn<Foo, DateTime?>.FieldExpression), foo.GenerateValueExpression(nameof(Foo.DateTime), typeof(DateTime?)));
        builder.AddAttribute(index++, nameof(TableColumn<Foo, DateTime?>.Filterable), true);
        builder.CloseComponent();

        builder.OpenComponent<TableColumn<Foo, IEnumerable<string>>>(index++);
        builder.AddAttribute(index++, nameof(TableColumn<Foo, IEnumerable<string>>.Field), foo.Hobby);
        builder.AddAttribute(index++, nameof(TableColumn<Foo, IEnumerable<string>>.Lookup), new List<SelectedItem>()
        {
            new SelectedItem("1", "Test1"),
            new SelectedItem("2", "Test2"),
        });
        builder.AddAttribute(index++, nameof(TableColumn<Foo, IEnumerable<string>>.FieldExpression), foo.GenerateValueExpression(nameof(Foo.Hobby), typeof(IEnumerable<string>)));
        builder.AddAttribute(index++, nameof(TableColumn<Foo, IEnumerable<string>>.Filterable), true);
        builder.CloseComponent();
    };
}

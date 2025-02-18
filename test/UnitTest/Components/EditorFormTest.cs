﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Shared;
using System.ComponentModel.DataAnnotations;

namespace UnitTest.Components;

public class EditorFormTest : BootstrapBlazorTestBase
{
    [Fact]
    public void CascadedEditContext_Error()
    {
        var foo = new Foo();
        Assert.ThrowsAny<InvalidOperationException>(() =>
        {
            Context.RenderComponent<ValidateForm>(pb =>
            {
                pb.Add(a => a.Model, foo);
                pb.AddChildContent<EditorForm<Dummy>>(pb =>
                {
                    pb.Add(a => a.Model, new Dummy());
                });
            });
        });
    }

    [Fact]
    public void CascadedEditContext_Ok()
    {
        var foo = new Foo();
        Context.RenderComponent<ValidateForm>(pb =>
        {
            pb.Add(a => a.Model, foo);
            pb.AddChildContent<EditorForm<Foo>>();
        });
    }

    [Fact]
    public void Model_Error()
    {
        Assert.ThrowsAny<ArgumentNullException>(() =>
        {
            Context.RenderComponent<EditorForm<Foo>>(pb =>
            {
                pb.Add(a => a.Model, null);
            });
        });
    }

    [Fact]
    public void Items_Ok()
    {
        var foo = new Foo();
        Context.RenderComponent<EditorForm<Foo>>(pb =>
        {
            pb.Add(a => a.Model, foo);
            pb.Add(a => a.Items, new List<MockTableColumn>
            {
                new("Id", typeof(int)),
                new("Name", typeof(string))
            });
        });
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void AutoGenerateAllItem_True(bool autoGenerate)
    {
        var foo = new Foo();
        var cut = Context.RenderComponent<EditorForm<Foo>>(pb =>
        {
            pb.Add(a => a.AutoGenerateAllItem, autoGenerate);
            pb.Add(a => a.Model, foo);
            pb.Add(a => a.FieldItems, GenerateEditorItems(foo));
        });
    }

    [Fact]
    public void IsDisplay_Ok()
    {
        var foo = new Foo();
        var cut = Context.RenderComponent<EditorForm<Foo>>(pb =>
        {
            pb.Add(a => a.IsDisplay, true);
            pb.Add(a => a.Model, foo);
            pb.Add(a => a.FieldItems, GenerateEditorItems(foo));
        });
    }

    [Fact]
    public void IsSearch_Ok()
    {
        var foo = new Foo();
        var cut = Context.RenderComponent<EditorForm<Foo>>(pb =>
        {
            pb.AddCascadingValue("IsSearch", true);
            pb.Add(a => a.Model, foo);
            pb.Add(a => a.FieldItems, GenerateEditorItems(foo));
        });
    }

    [Fact]
    public void Buttons_Ok()
    {
        var foo = new Foo();
        var cut = Context.RenderComponent<EditorForm<Foo>>(pb =>
        {
            pb.Add(a => a.Model, foo);
            pb.Add(a => a.Buttons, builder =>
            {
                builder.OpenComponent<Button>(0);
                builder.CloseComponent();
            });
        });
    }

    [Fact]
    public void Misc_Ok()
    {
        var foo = new Foo();
        var cut = Context.RenderComponent<EditorForm<Foo>>(pb =>
        {
            pb.Add(a => a.Model, foo);
            pb.Add(a => a.ItemsPerRow, 1);
            pb.Add(a => a.ItemChangedType, ItemChangedType.Add);
            pb.Add(a => a.RowType, RowType.Inline);
            pb.Add(a => a.LabelAlign, Alignment.Right);
        });
        cut.Contains("row g-3 form-inline is-end");
        cut.Contains("col-12");
    }

    [Fact]
    public void FieldChanged_Ok()
    {
        var cut = Context.RenderComponent<MockEditorItem>(pb =>
        {
            pb.Add(a => a.Field, "Nama");
            pb.Add(a => a.FieldChanged, EventCallback.Factory.Create<string>(this, v => { }));
        });
        cut.InvokeAsync(() => cut.Instance.Test());
    }

    [Fact]
    public void DisplayName_Ok()
    {
        var cut = Context.RenderComponent<EditorItem<Foo, string>>();
        Assert.Equal("", cut.Instance.GetDisplayName());
        Assert.Equal("", cut.Instance.GetFieldName());
    }

    [Fact]
    public void EditTemplate_Coverage()
    {
        var editorItem = new EditorItem<Foo, string>();
        IEditorItem item = editorItem;
        item.EditTemplate = null;
        Assert.Null(editorItem.Field);
    }

    [Fact]
    public void IsEditable_Ok()
    {
        var editorItem = new EditorItem<Foo, string>()
        {
            IsReadonlyWhenAdd = true,
            IsReadonlyWhenEdit = false
        };
        editorItem.SetParametersAsync(ParameterView.FromDictionary(new Dictionary<string, object?>
        {
            ["Editable"] = true,
            ["Readonly"] = false,
        }));
        Assert.False(editorItem.IsEditable(ItemChangedType.Add));
        Assert.True(editorItem.IsEditable(ItemChangedType.Update));
    }

    [Fact]
    public void EditorItem_Ok()
    {
        var foo = new Foo();
        var cut = Context.RenderComponent<ValidateForm>(pb =>
        {
            pb.Add(a => a.Model, foo);
            pb.AddChildContent<EditorForm<Foo>>(pb =>
            {
                pb.Add(a => a.AutoGenerateAllItem, false);
                pb.Add(a => a.FieldItems, f => builder =>
                {
                    var index = 0;
                    builder.OpenComponent<EditorItem<Foo, string>>(index++);
                    builder.AddAttribute(index++, nameof(EditorItem<Foo, string>.Field), f.Name);
                    builder.AddAttribute(index++, nameof(EditorItem<Foo, string>.FieldExpression), Utility.GenerateValueExpression(foo, nameof(Foo.Name), typeof(string)));
                    builder.AddAttribute(index++, nameof(EditorItem<Foo, string>.Readonly), true);
                    builder.AddAttribute(index++, nameof(EditorItem<Foo, string>.SkipValidate), false);
                    builder.AddAttribute(index++, nameof(EditorItem<Foo, string>.Text), "Test-Text");
                    builder.AddAttribute(index++, nameof(EditorItem<Foo, string>.ComponentType), typeof(BootstrapInput<string>));
                    builder.AddAttribute(index++, nameof(EditorItem<Foo, string>.ComponentParameters), new KeyValuePair<string, object>[]
                    {
                        new("type", "text")
                    });
                    builder.AddAttribute(index++, nameof(EditorItem<Foo, string>.ValidateRules), new List<IValidator>
                    {
                        new FormItemValidator(new RequiredAttribute())
                    });
                    builder.CloseComponent();

                    builder.OpenComponent<EditorItem<Foo, string>>(index++);
                    builder.AddAttribute(index++, nameof(EditorItem<Foo, string>.Field), f.Address);
                    builder.AddAttribute(index++, nameof(EditorItem<Foo, string>.FieldExpression), Utility.GenerateValueExpression(foo, nameof(Foo.Address), typeof(string)));
                    builder.AddAttribute(index++, nameof(EditorItem<Foo, string>.Rows), 3);
                    builder.CloseComponent();

                    builder.OpenComponent<EditorItem<Foo, int>>(index++);
                    builder.AddAttribute(index++, nameof(EditorItem<Foo, int>.Field), f.Count);
                    builder.AddAttribute(index++, nameof(EditorItem<Foo, int>.FieldExpression), Utility.GenerateValueExpression(foo, nameof(Foo.Count), typeof(int)));
                    builder.AddAttribute(index++, nameof(EditorItem<Foo, int>.Step), 3);
                    builder.CloseComponent();

                    builder.OpenComponent<EditorItem<Foo, bool>>(index++);
                    builder.AddAttribute(index++, nameof(EditorItem<Foo, bool>.Field), f.Complete);
                    builder.AddAttribute(index++, nameof(EditorItem<Foo, bool>.FieldExpression), Utility.GenerateValueExpression(foo, nameof(Foo.Complete), typeof(bool)));
                    builder.AddAttribute(index++, nameof(EditorItem<Foo, bool>.Items), new List<SelectedItem>
                    {
                        new("True", "Test-True"),
                        new("False", "Test-False")
                    });
                    builder.AddAttribute(index++, nameof(EditorItem<Foo, bool>.Lookup), new List<SelectedItem>
                    {
                        new("True", "Test-True"),
                        new("False", "Test-False")
                    });
                    builder.CloseComponent();
                });
            });
        });
    }

    private static RenderFragment<Foo> GenerateEditorItems(Foo foo) => f => builder =>
    {
        builder.OpenComponent<EditorItem<Foo, string>>(0);
        builder.AddAttribute(1, nameof(EditorItem<Foo, string>.Field), f.Name);
        builder.AddAttribute(2, nameof(EditorItem<Foo, string>.FieldExpression), Utility.GenerateValueExpression(foo, nameof(Foo.Name), typeof(string)));
        builder.AddAttribute(3, nameof(EditorItem<Foo, string>.EditTemplate), new RenderFragment<Foo>(f => b =>
        {
            b.OpenComponent<Button>(0);
            b.CloseComponent();
        }));
        builder.CloseComponent();

        builder.OpenComponent<EditorItem<Foo, string>>(0);
        builder.AddAttribute(1, nameof(EditorItem<Foo, string>.Field), f.Address);
        builder.AddAttribute(2, nameof(EditorItem<Foo, string>.FieldExpression), Utility.GenerateValueExpression(foo, nameof(Foo.Address), typeof(string)));
        builder.AddAttribute(3, nameof(EditorItem<Foo, string>.Editable), false);
        builder.CloseComponent();
    };

    private class Dummy
    {
        public string? Name { get; set; }
    }

    private class MockEditorItem : EditorItem<Foo, string>
    {
        public async Task Test()
        {
            if (FieldChanged.HasDelegate)
            {
                await FieldChanged.InvokeAsync();
            }
        }
    }
}

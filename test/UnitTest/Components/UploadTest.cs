﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Shared;
using Microsoft.AspNetCore.Components.Forms;

namespace UnitTest.Components;

public class UploadTest : BootstrapBlazorTestBase
{
    [Fact]
    public void InputUpload_Ok()
    {
        UploadFile? uploadFile = null;
        var cut = Context.RenderComponent<InputUpload<string>>(pb =>
        {
            pb.Add(a => a.PlaceHolder, "TestPlaceHolder");
            pb.Add(a => a.OnChange, file =>
            {
                uploadFile = file;
                return Task.CompletedTask;
            });
        });
        var input = cut.FindComponent<InputFile>();
        cut.InvokeAsync(() => input.Instance.OnChange.InvokeAsync(new InputFileChangeEventArgs(new List<MockBrowserFile>()
        {
            new MockBrowserFile()
        })));
        Assert.Equal("UploadTestFile", uploadFile!.OriginFileName);
        cut.Contains("fa fa-folder-open-o");
        cut.Contains("btn-primary");
        cut.Contains("TestPlaceHolder");

        // 参数
        cut.SetParametersAndRender(pb => pb.Add(a => a.BrowserButtonIcon, "fa fa-browser"));
        cut.Contains("fa fa-browser");

        cut.SetParametersAndRender(pb => pb.Add(a => a.BrowserButtonClass, "btn btn-browser"));
        cut.Contains("btn btn-browser");

        cut.SetParametersAndRender(pb =>
        {
            pb.Add(a => a.ShowDeleteButton, true);
            pb.Add(a => a.DeleteButtonText, "Delete-Test");
            pb.Add(a => a.DeleteButtonIcon, "fa fa-delete-icon");
        });
        cut.Contains("fa fa-delete-icon");
        cut.Contains("btn-danger");

        // 删除逻辑
        var deleted = false;
        cut.SetParametersAndRender(pb =>
        {
            pb.Add(a => a.DeleteButtonClass, "btn btn-delete");
            pb.Add(a => a.OnDelete, file =>
            {
                deleted = true;
                return Task.FromResult(true);
            });
        });
        cut.Contains("btn btn-delete");

        var button = cut.Find(".input-group button");
        cut.InvokeAsync(() => button.Click());
        Assert.True(deleted);

        // IsDisable
        cut.SetParametersAndRender(pb =>
        {
            pb.Add(a => a.IsDisabled, true);
        });
        cut.Contains("btn btn-delete");
    }

    [Fact]
    public void InputUpload_ValidateForm_Ok()
    {
        var invalid = false;
        var foo = new Foo();
        var cut = Context.RenderComponent<ValidateForm>(pb =>
        {
            pb.Add(a => a.Model, foo);
            pb.AddChildContent<InputUpload<string>>(pb =>
            {
                pb.Add(a => a.Value, foo.Name);
                pb.Add(a => a.ValueExpression, foo.GenerateValueExpression());
            });
            pb.Add(a => a.OnValidSubmit, context =>
            {
                invalid = false;
                return Task.CompletedTask;
            });
            pb.Add(a => a.OnInvalidSubmit, context =>
            {
                invalid = true;
                return Task.CompletedTask;
            });
        });

        // 提交表单
        var form = cut.Find("form");
        cut.InvokeAsync(() => form.Submit());
        Assert.True(invalid);

        var input = cut.FindComponent<InputFile>();
        cut.InvokeAsync(() => input.Instance.OnChange.InvokeAsync(new InputFileChangeEventArgs(new List<MockBrowserFile>()
        {
            new MockBrowserFile()
        })));
        cut.InvokeAsync(() => form.Submit());
        Assert.False(invalid);
    }

    [Fact]
    public void AvatarUpload_Ok()
    {
        UploadFile? uploadFile = null;
        var cut = Context.RenderComponent<AvatarUpload<string>>(pb =>
        {
            pb.Add(a => a.IsSingle, true);
            pb.Add(a => a.OnChange, file =>
            {
                uploadFile = file;
                return Task.CompletedTask;
            });
        });
        var input = cut.FindComponent<InputFile>();
        cut.InvokeAsync(() => input.Instance.OnChange.InvokeAsync(new InputFileChangeEventArgs(new List<MockBrowserFile>()
        {
            new MockBrowserFile()
        })));

        // Height/Width
        cut.SetParametersAndRender(pb =>
        {
            pb.Add(a => a.Height, 40);
            pb.Add(a => a.Width, 50);
        });
        cut.Contains("width: 50px;");
        cut.Contains("height: 40px;");

        cut.SetParametersAndRender(pb =>
        {
            pb.Add(a => a.IsCircle, true);
        });
        cut.Contains("height: 50px;");

        cut.SetParametersAndRender(pb =>
        {
            pb.Add(a => a.Height, 0);
        });
        cut.Contains("height: 50px;");

        // DefaultFileList
        cut.SetParametersAndRender(pb =>
        {
            pb.Add(a => a.OnChange, null);
            pb.Add(a => a.ShowProgress, true);
            pb.Add(a => a.DefaultFileList, new List<UploadFile>()
            {
                new UploadFile() { FileName  = "Test-File" }
            });
        });
        input = cut.FindComponent<InputFile>();
        cut.InvokeAsync(() => input.Instance.OnChange.InvokeAsync(new InputFileChangeEventArgs(new List<MockBrowserFile>()
        {
            new MockBrowserFile()
        })));

        // upload-item-delete
        var button = cut.Find(".upload-item-delete");
        cut.InvokeAsync(() => button.Click());

        // isdisable
        cut.SetParametersAndRender(pb =>
        {
            pb.Add(a => a.IsDisabled, true);
        });
    }

    [Fact]
    public void AvatarUpload_ValidateForm_Ok()
    {
        var invalid = false;
        var foo = new Foo();
        var cut = Context.RenderComponent<ValidateForm>(pb =>
        {
            pb.Add(a => a.Model, foo);
            pb.AddChildContent<AvatarUpload<string>>(pb =>
            {
                pb.Add(a => a.Value, foo.Name);
                pb.Add(a => a.ValueExpression, foo.GenerateValueExpression());
            });
            pb.Add(a => a.OnValidSubmit, context =>
            {
                invalid = false;
                return Task.CompletedTask;
            });
            pb.Add(a => a.OnInvalidSubmit, context =>
            {
                invalid = true;
                return Task.CompletedTask;
            });
        });

        // 提交表单
        var form = cut.Find("form");
        cut.InvokeAsync(() => form.Submit());
        Assert.True(invalid);

        var input = cut.FindComponent<InputFile>();
        cut.InvokeAsync(() => input.Instance.OnChange.InvokeAsync(new InputFileChangeEventArgs(new List<MockBrowserFile>()
        {
            new MockBrowserFile()
        })));
        cut.InvokeAsync(() => form.Submit());
        Assert.False(invalid);
    }

    [Fact]
    public void ButtonUpload_Ok()
    {
        UploadFile? uploadFile = null;
        var cut = Context.RenderComponent<ButtonUpload<string>>(pb =>
        {
            pb.Add(a => a.IsSingle, true);
            pb.Add(a => a.BrowserButtonClass, "browser-class");
            pb.Add(a => a.BrowserButtonIcon, "fa fa-browser-icon");
        });
        cut.Contains("fa fa-browser-icon");
        cut.Contains("browser-class");
        cut.DoesNotContain("form-label");

        // DefaultFileList
        cut.SetParametersAndRender(pb =>
        {
            pb.Add(a => a.ShowProgress, true);
            pb.Add(a => a.DefaultFileList, new List<UploadFile>()
            {
                new UploadFile() { FileName  = "Test-File" }
            });
            pb.Add(a => a.OnChange, file =>
            {
                uploadFile = file;
                return Task.CompletedTask;
            });
        });
        var input = cut.FindComponent<InputFile>();
        cut.InvokeAsync(() => input.Instance.OnChange.InvokeAsync(new InputFileChangeEventArgs(new List<MockBrowserFile>()
        {
            new MockBrowserFile()
        })));
    }

    [Fact]
    public void ButtonUpload_IsDisabled_Ok()
    {
        var cut = Context.RenderComponent<ButtonUpload<string>>(pb =>
        {
            pb.Add(a => a.IsDisabled, true);
        });
        var button = cut.Find(".btn-browser");
        Assert.Contains("disabled=\"disabled\"", button.ToMarkup());

        cut.SetParametersAndRender(pb =>
        {
            pb.Add(a => a.IsDisabled, false);
            pb.Add(a => a.IsSingle, false);
        });
        Assert.DoesNotContain("disabled", button.ToMarkup());

        cut.SetParametersAndRender(pb =>
        {
            pb.Add(a => a.IsDisabled, false);
            pb.Add(a => a.IsSingle, true);
        });
        Assert.DoesNotContain("disabled", button.ToMarkup());

        cut.SetParametersAndRender(pb =>
        {
            pb.Add(a => a.IsDisabled, false);
            pb.Add(a => a.IsSingle, true);
        });
        var input = cut.FindComponent<InputFile>();
        cut.InvokeAsync(() => input.Instance.OnChange.InvokeAsync(new InputFileChangeEventArgs(new List<MockBrowserFile>()
        {
            new MockBrowserFile()
        })));
        Assert.Contains("disabled=\"disabled\"", button.ToMarkup());
    }

    [Fact]
    public void ButtonUpload_ValidateForm_Ok()
    {
        var foo = new Foo();
        var cut = Context.RenderComponent<ValidateForm>(pb =>
        {
            pb.Add(a => a.Model, foo);
            pb.AddChildContent<ButtonUpload<string>>(pb =>
            {
                pb.Add(a => a.Value, foo.Name);
                pb.Add(a => a.ValueExpression, foo.GenerateValueExpression());
            });
        });
        cut.Contains("form-label");
    }

    [Fact]
    public void ButtonUpload_OnDeleteFile_Ok()
    {
        UploadFile? deleteFile = null;
        var cut = Context.RenderComponent<ButtonUpload<string>>(pb =>
        {
            pb.Add(a => a.IsSingle, false);
            pb.Add(a => a.DefaultFileList, new List<UploadFile>()
            {
                new UploadFile() { FileName  = "Test-File" }
            });
            pb.Add(a => a.OnDelete, file =>
            {
                deleteFile = file;
                return Task.FromResult(true);
            });
        });
        cut.InvokeAsync(() => cut.Find(".fa-trash-o.text-danger").Click());
        Assert.NotNull(deleteFile);

        deleteFile = null;
        // 上传失败测试
        cut.SetParametersAndRender(pb =>
        {
            pb.Add(a => a.DefaultFileList, new List<UploadFile>()
            {
                new UploadFile() { FileName  = "Test-File2", Code = 1001 }
            });
        });
        cut.InvokeAsync(() => cut.Find(".fa-trash-o.text-danger").Click());
        Assert.NotNull(deleteFile);
    }

    [Fact]
    public void ButtonUpload_ShowProgress_Ok()
    {
        var cut = Context.RenderComponent<ButtonUpload<string>>(pb =>
        {
            pb.Add(a => a.ShowProgress, true);
            pb.Add(a => a.OnChange, async file =>
            {
                await file.SaveToFile("1.txt");
            });
        });
        var input = cut.FindComponent<InputFile>();
        cut.InvokeAsync(() => input.Instance.OnChange.InvokeAsync(new InputFileChangeEventArgs(new List<MockBrowserFile>()
        {
            new MockBrowserFile()
        })));
    }

    [Fact]
    public void ButtonUpload_IsDirectory_Ok()
    {
        var fileNames = new List<string>();
        var cut = Context.RenderComponent<ButtonUpload<string>>(pb =>
        {
            pb.Add(a => a.IsDirectory, true);
            pb.Add(a => a.OnChange, file =>
            {
                fileNames.Add(file.OriginFileName!);
                return Task.CompletedTask;
            });
        });
        var input = cut.FindComponent<InputFile>();
        cut.InvokeAsync(() => input.Instance.OnChange.InvokeAsync(new InputFileChangeEventArgs(new List<MockBrowserFile>()
        {
            new MockBrowserFile(),
            new MockBrowserFile("UploadTestFile2")
        })));
        Assert.Equal(2, fileNames.Count);
    }

    [Fact]
    public void ButtonUpload_OnGetFileFormat_Ok()
    {
        var cut = Context.RenderComponent<ButtonUpload<string>>(pb =>
        {
            pb.Add(a => a.DefaultFileList, new List<UploadFile>()
            {
                new UploadFile() { FileName  = "1.csv" },
                new UploadFile() { FileName  = "1.xls" },
                new UploadFile() { FileName  = "1.xlsx" },
                new UploadFile() { FileName  = "1.doc" },
                new UploadFile() { FileName  = "1.docx" },
                new UploadFile() { FileName  = "1.dot" },
                new UploadFile() { FileName  = "1.ppt" },
                new UploadFile() { FileName  = "1.pptx" },
                new UploadFile() { FileName  = "1.wav" },
                new UploadFile() { FileName  = "1.mp3" },
                new UploadFile() { FileName  = "1.mp4" },
                new UploadFile() { FileName  = "1.mov" },
                new UploadFile() { FileName  = "1.mkv" },
                new UploadFile() { FileName  = "1.cs" },
                new UploadFile() { FileName  = "1.html" },
                new UploadFile() { FileName  = "1.vb" },
                new UploadFile() { FileName  = "1.pdf" },
                new UploadFile() { FileName  = "1.zip" },
                new UploadFile() { FileName  = "1.rar" },
                new UploadFile() { FileName  = "1.iso" },
                new UploadFile() { FileName  = "1.txt" },
                new UploadFile() { FileName  = "1.log" },
                new UploadFile() { FileName  = "1.jpg" },
                new UploadFile() { FileName  = "1.jpeg" },
                new UploadFile() { FileName  = "1.png" },
                new UploadFile() { FileName  = "1.bmp" },
                new UploadFile() { FileName  = "1.gif" },
                new UploadFile() { FileName  = "1.test" }
            });
        });
        cut.Contains("fa-file-excel-o");
        cut.Contains("fa-file-word-o");
        cut.Contains("fa-file-powerpoint-o");
        cut.Contains("fa-file-audio-o");
        cut.Contains("fa-file-video-o");
        cut.Contains("fa-file-code-o");
        cut.Contains("fa-file-pdf-o");
        cut.Contains("fa-file-archive-o");
        cut.Contains("fa-file-text-o");
        cut.Contains("fa-file-image-o");
        cut.Contains("fa-file-o");

        cut.SetParametersAndRender(pb =>
        {
            pb.Add(a => a.OnGetFileFormat, extensions =>
            {
                return "fa-format-test";
            });
        });
        cut.Contains("fa-format-test");
    }

    [ExcludeFromCodeCoverage]
    private class MockBrowserFile : IBrowserFile
    {
        public MockBrowserFile(string name = "UploadTestFile")
        {
            Name = name;
            LastModified = DateTimeOffset.Now;
            Size = 10;
            ContentType = "jpg";
        }

        public string Name { get; }

        public DateTimeOffset LastModified { get; }

        public long Size { get; }

        public string ContentType { get; }

        public Stream OpenReadStream(long maxAllowedSize = 512000, CancellationToken cancellationToken = default)
        {
            return new MemoryStream(new byte[] { 0x01, 0x02 });
        }
    }
}

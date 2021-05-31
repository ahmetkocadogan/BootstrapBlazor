﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;

namespace BootstrapBlazor.Components
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public abstract class SingleUploadBase<TValue> : MultipleUploadBase<TValue>
    {
        /// <summary>
        /// 获得/设置 是否仅上传一次 默认 false
        /// </summary>
        [Parameter]
        public bool IsSingle { get; set; }

        /// <summary>
        /// 获得当前图片集合
        /// </summary>
        /// <returns></returns>
        protected override List<UploadFile> GetUploadFiles()
        {
            var ret = new List<UploadFile>();
            if (IsSingle)
            {
                if (DefaultFileList.Any())
                {
                    ret.Add(DefaultFileList.First());
                }
                if (ret.Count == 0 && UploadFiles.Any())
                {
                    ret.Add(UploadFiles.First());
                }
            }
            else
            {
                ret.AddRange(DefaultFileList);
                ret.AddRange(UploadFiles);
            }
            return ret;
        }
    }
}

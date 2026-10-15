#if UNITY_WEBGL && ENABLE_ALIPAY_MINI_GAME && ENABLE_ALIPAY_MINI_GAME_OPERATIONCLIPBOARD

// ==========================================================================================
//   GameFrameX 组织及其衍生项目的版权、商标、专利及其他相关权利
//   GameFrameX organization and its derivative projects' copyrights, trademarks, patents, and related rights
//   均受中华人民共和国及相关国际法律法规保护。
//   are protected by the laws of the People's Republic of China and relevant international regulations.
//   使用本项目须严格遵守相关法律法规及开源许可证之规定。
//   Usage of this project must strictly comply with applicable laws, regulations, and open-source licenses.
//   本项目采用 MIT 许可证与 Apache License 2.0 双许可证分发，
//   This project is dual-licensed under the MIT License and Apache License 2.0,
//   完整许可证文本请参见源代码根目录下的 LICENSE 文件。
//   please refer to the LICENSE file in the root of the source tree for the full license text.
//   禁止利用本项目实施任何危害国家安全、破坏社会秩序、
//   It is prohibited to use this project to engage in any activities that endanger national security, disrupt social order,
//   侵犯他人合法权益等法律法规所禁止的行为！
//   or infringe upon the legitimate rights and interests of others, as prohibited by laws and regulations!
//   因基于本项目二次开发所产生的一切法律纠纷与责任，
//   Any disputes or liabilities arising from secondary development based on this project
//   本项目组织及贡献者概不承担。
//   shall be borne solely by the developer; the project organization and contributors assume no responsibility.
//   GitHub 仓库：https://github.com/GameFrameX
//   GitHub Repository: https://github.com/GameFrameX
//   Gitee  仓库：https://gitee.com/GameFrameX
//   Gitee Repository:  https://gitee.com/GameFrameX
//   CNB  仓库：https://cnb.cool/GameFrameX
//   CNB Repository:    https://cnb.cool/GameFrameX
//   官方文档：https://gameframex.doc.alianblank.com/
//   Official Documentation: https://gameframex.doc.alianblank.com/
//  ==========================================================================================

using System;
using AlipaySdk;
using GameFrameX.OperationClipboard.Runtime;
using UnityEngine;

namespace GameFrameX.OperationClipboard.AlipayMiniGame.Runtime
{
    /// <summary>
    /// 支付宝小游戏粘贴板管理器。
    /// </summary>
    /// <remarks>
    /// 支付宝剪贴板 API 为 JSON 字符串回调：成功时空对象或含 text 字段，失败时含 error / errorMessage 字段
    /// （error=4 为个人主体无权限，30/60000 为系统剪贴板权限拒绝，读取仅企业主体小程序可用）。
    /// 用 <see cref="JsonUtility"/> + 可序列化包装解析，不引入 JSON 依赖；解析异常按失败交付，不向 SDK 回调抛出。
    /// </remarks>
    [UnityEngine.Scripting.Preserve]
    public sealed class AliPayMiniGameOperationClipboardManager : IOperationClipboardManager
    {
        /// <summary>
        /// 渠道唯一标识
        /// </summary>
        public const string ChannelId = "alipayminigame";

        /// <summary>
        /// 支付宝剪贴板回调 JSON 包装：成功为 {}（写）或 {"text":...}（读），失败含 error / errorMessage。
        /// </summary>
        [Serializable]
        private class AlipayClipboardResult
        {
            /// <summary>读取到的剪贴板文本</summary>
            public string text;

            /// <summary>错误码，0 表示无错误</summary>
            public int error;

            /// <summary>错误信息，非空表示失败</summary>
            public string errorMessage;
        }

        /// <summary>
        /// 读取粘贴板内容（异步回调；仅企业主体小程序可用，个人主体回调失败）。
        /// </summary>
        /// <param name="callback">结果回调：成功标志与文本内容。</param>
        [UnityEngine.Scripting.Preserve]
        public void GetValue(Action<bool, string> callback)
        {
            if (callback == null)
            {
                return;
            }

            AP.GetClipboard(json =>
            {
                var result = Parse(json);
                callback.Invoke(result != null && IsSuccess(result), result != null ? result.text ?? string.Empty : string.Empty);
            });
        }

        /// <summary>
        /// 写入粘贴板内容（异步回调）。
        /// </summary>
        /// <param name="text">要写入的文本内容。</param>
        /// <param name="callback">可选结果回调。</param>
        [UnityEngine.Scripting.Preserve]
        public void SetValue(string text, Action<bool> callback = null)
        {
            AP.SetClipboard(text ?? string.Empty, json =>
            {
                var result = Parse(json);
                callback?.Invoke(result != null && IsSuccess(result));
            });
        }

        private static AlipayClipboardResult Parse(string json)
        {
            try
            {
                return JsonUtility.FromJson<AlipayClipboardResult>(string.IsNullOrEmpty(json) ? "{}" : json);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static bool IsSuccess(AlipayClipboardResult result)
        {
            return result.error == 0 && string.IsNullOrEmpty(result.errorMessage);
        }
    }
}

#endif

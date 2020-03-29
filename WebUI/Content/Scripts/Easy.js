(function ($) {
    //全局系统对象
    window["Easy"] = {};

    //操作cookies
    Easy.cookies = (function () {
        var fn = function () {
        };
        fn.prototype.get = function (name) {
            var cookieValue = "";
            var search = name + "=";
            if (document.cookie.length > 0) {
                offset = document.cookie.indexOf(search);
                if (offset != -1) {
                    offset += search.length;
                    end = document.cookie.indexOf(";", offset);
                    if (end === -1) end = document.cookie.length;
                    cookieValue = decodeURIComponent(document.cookie.substring(offset, end))
                }
            }
            return cookieValue;
        };
        fn.prototype.set = function (cookieName, cookieValue, DayValue) {
            var expire = "";
            var day_value = 1;
            if (DayValue != null) {
                day_value = DayValue;
            }
            expire = new Date((new Date()).getTime() + day_value * 86400000);
            expire = "; expires=" + expire.toGMTString();
            document.cookie = cookieName + "=" + encodeURIComponent(cookieValue) + ";path=/" + expire;
        }
        fn.prototype.remove = function (cookieName) {
            var expire = "";
            expire = new Date((new Date()).getTime() - 1);
            expire = "; expires=" + expire.toGMTString();
            document.cookie = cookieName + "=" + escape("") + ";path=/" + expire;
            /*path=/*/
        };

        return new fn();
    })();

    //设置表单tooltip，显示文字，点击消失
    Easy.toolTip = function () {
        $("input[tooltip]").each(function (i, n) {
            var toolTip = $(n).attr("tooltip");
            var type = $(n).attr("type");
            var n2 = $("<input type='text' />");

            if ($(n).val() === "") {
                if (type === "password") {
                    n2.attr("class", $(n).attr("class"));
                    n2.val(toolTip);
                    n2.css("color", "#9b9b9f");
                    n2.focus(function () {
                        $(n).show().focus();
                        n2.hide();
                    });
                    $(n).after(n2);
                    $(n).hide();
                } else {
                    $(n).val(toolTip);
                    $(n).css("color", "#9b9b9f");
                }
            } else {
                $(n).css("color", "#000");
            }

            $(n).focusin(function () {
                $(n).css("color", "#000");
                if ($(n).val() === toolTip) {
                    $(n).val("");
                    $(n).css("color", "#000");
                }
            });

            $(n).focusout(function () {
                if (type === "password" && $(n).val() === "") {
                    n2.show();
                    $(n).hide();
                }
                if ($(n).val() === "") {
                    $(n).val(toolTip);
                    $(n).css("color", "#9b9b9f");
                } else {
                    $(n).css("color", "#000");
                }
            });

            $("form").submit(function () {
                if ($(n).val() === toolTip) {
                    $(n).val("");

                    if (type === "password" && n2.val() === toolTip) {
                        $(n2).val("");
                    }
                }
            });
        });
    };

    //验证输入值（提交地址、提醒内容）
    Easy.checkValue = function (jsondata) {
        var isWarn = true;
        var url = "";
        var id = "";//排除自己ID
        var typeId = "";//只在所在分类中查询
        var type = "";//1:下拉框2:树3:文本框
        $.extend($.fn.validatebox.defaults.rules, {
            //判断是否和数据库匹配
            isRight: {
                validator: function (value, param) {
                    url = jsondata.jsontext[param[0]].url;
                    id = jsondata.jsontext[param[0]].id;
                    var idValue = $(id).val() === "" ? "0" : $(id).val();
                    var typeIdValue = "";
                    var num = jsondata.jsontext[param[0]].type.length;
                    for (var i = 0; i < num; i++) {
                        typeId = jsondata.jsontext[param[0]].type[i].typeId;
                        type = jsondata.jsontext[param[0]].type[i].type;
                        if (type === "1") {
                            typeIdValue += $(typeId).combobox("getValue") + ",";
                        }
                        if (type === "2") {
                            typeIdValue += $(typeId).combotree("getValue") + ",";
                        }
                        if (type === "3") {
                            typeIdValue += $(typeId).textbox("getValue") + ",";
                        }
                    }
                    if (typeIdValue != "") {
                        typeIdValue = typeIdValue.substring(0, typeIdValue.length - 1);
                    }
                    $.ajax({
                        type: "post",
                        url: url,
                        async: false,
                        data: { Value: value, ID: idValue, TypeId: typeIdValue },
                        dataType: "json",
                        success: function (result) {
                            if (result.IsError === true) {
                                isWarn = true;
                            }
                            else {
                                isWarn = false;
                            }
                        },
                        error: function () {
                            isWarn = true;
                        }
                    });
                    if (isWarn) {
                        return false;
                    }
                    else {
                        return true;
                    }
                },
                message: "{1}"
            }
        });
    }

    //验证隐藏于是否为默认值
    Easy.checkHiddenNull = function (jsondata) {
        var defaultValue = "";
        var id = "";
        $.extend($.fn.validatebox.defaults.rules, {
            isHiddenNull: {
                validator: function (value, param) {
                    defaultValue = jsondata.jsontext[param[0]].defaultValue;
                    id = jsondata.jsontext[param[0]].id;
                    if (value === defaultValue) {
                        $(id).tooltip("show");//必须调用Easy.bindCustomPromptEvent
                        return false;
                    }
                    else {
                        $(id).tooltip("hide");//必须调用Easy.bindCustomPromptEvent
                        return true;
                    }
                },
                message: ""
            }
        });
    }

    //验证两次数据输入是否相同（提醒内容）
    Easy.checkPwdAndOKPwdIsEqualTo = function (warnText) {
        $.extend($.fn.validatebox.defaults.rules, {
            //判断必须和某个字段相同
            equalTo: {
                validator: function (value, param) {
                    return $("#" + param[0]).val() === value;
                },
                message: warnText
            }
        });
    }

    //点击保存按钮（弹出窗口id,表单id,提交地址,提交按钮id,刷新类别(1:表格2:树3:方法4:treegrid),刷新id(如类别是3则是方法名),菜单id,按钮分组,主键id）
    Easy.bindSaveButtonClickEvent = function (dialog, form, url, btn, type, loadId, menuId, num, id) {
        $(btn).click(function () {
            if ($(btn).linkbutton("options").disabled === false) {//按钮没有禁用才能执行（easyui特殊处理）
                var buttonCode = "";
                if ($(id).val() === "") {
                    buttonCode = "add";
                }
                else {
                    buttonCode = "edit";
                }
                if (Easy.bindPowerValidationEvent(menuId, num, buttonCode)) {
                    $(form).form("submit", {
                        url: url,
                        onSubmit: function () {
                            var validate = $(form).form("validate");//验证
                            if (validate) {
                                $(btn).linkbutton("disable");//禁用按钮
                            }
                            return validate;
                        },
                        success: function (result) {
                            if (result === "yes") {
                                $(dialog).dialog("close");//关闭弹窗
                                Easy.centerShow("系统消息", "保存成功", 3000);
                                if (loadId != "") {
                                    if (type === "1") {
                                        $(loadId).datagrid("load");//刷新表格
                                    }
                                    if (type === "2") {
                                        $(loadId).tree("reload");//刷新树
                                    }
                                    if (type === "3") {
                                        loadId();
                                    }
                                    if (type === "4") {
                                        $(loadId).treegrid("load");
                                    }
                                }
                            }
                            else {
                                Easy.centerShow("系统消息", result, 3000);
                            }
                            $(btn).linkbutton("enable");//解除按钮禁用
                        }
                    });
                }
            }
        });
    }

    //点击保存按钮（弹出窗口id,表单id,提交地址,提交按钮id,刷新类别(1:表格2:树3:方法4:treegrid),刷新id(如类别是3则是方法名),菜单id,按钮分组,按钮权限code）
    Easy.bindSaveButtonClickEventBybtnCode = function (dialog, form, url, btn, type, loadId, menuId, num, btnCode) {
        $(btn).click(function () {
            if ($(btn).linkbutton("options").disabled === false) {//按钮没有禁用才能执行（easyui特殊处理）
                var buttonCode = btnCode;
                if (Easy.bindPowerValidationEvent(menuId, num, buttonCode)) {
                    $(form).form("submit", {
                        url: url,
                        onSubmit: function () {
                            var validate = $(form).form("validate");//验证
                            if (validate) {
                                $(btn).linkbutton("disable");//禁用按钮
                            }
                            return validate;
                        },
                        success: function (result) {
                            if (result === "yes") {
                                $(dialog).dialog("close");//关闭弹窗
                                Easy.centerShow("系统消息", "保存成功", 3000);
                                if (loadId != "") {
                                    if (type === "1") {
                                        $(loadId).datagrid("load");//刷新表格
                                    }
                                    if (type === "2") {
                                        $(loadId).tree("reload");//刷新树
                                    }
                                    if (type === "3") {
                                        loadId();
                                    }
                                    if (type === "4") {
                                        $(loadId).treegrid("load");
                                    }
                                }
                            }
                            else {
                                Easy.centerShow("系统消息", result, 3000);
                            }
                            $(btn).linkbutton("enable");//解除按钮禁用
                        }
                    });
                }
            }
        });
    }

    //绑定弹出窗口事件
    Easy.OpenDialogEvent = function (dialog, title, width, height, url, buttonid) {
        $(dialog).dialog({
            title: title,
            width: width,
            height: height,
            href: url,
            buttons: buttonid,
            modal: true
        });
    }

    //右下角提示信息
    Easy.rightShow = function (title, msg, timeout) {
        $.messager.show({
            title: title,
            msg: msg,
            timeout: timeout,
            showType: "slide"
        });
    }

    //正上方提示信息
    Easy.centerShow = function (title, msg, timeout) {
        parent.$.messager.show({
            title: title,
            msg: msg,
            timeout: timeout,
            showType: "slide",
            style: {
                top: document.body.scrollTop + document.documentElement.scrollTop + 72
            }
        });
    }

    //加载表格按钮
    Easy.loadToolbar = function (MenuID, Num) {
        var tool = [];
        $.ajax({
            type: "post",
            url: "../Button/GetMyButton",
            async: false,
            data: { MenuID: MenuID, Num: Num },
            dataType: "json",
            success: function (result) {
                for (var i = 0, l = result.Data.length; i < l; i++) {
                    var o = result.Data[i];
                    if (i > 0) {
                        tool[tool.length] = "-";
                    }
                    tool[tool.length] = {
                        text: o.Name,
                        iconCls: o.IconPath,
                        handler: o.Code,
                        width: "auto"
                    };
                }
            },
            error: function () {
                Easy.centerShow("系统消息", "按钮信息加载失败", 3000);
            }
        });
        return tool;
    }
    Easy.loadToolbarHtml = function (MenuID, Num) {
        var tool = "";
        $.ajax({
            type: "post",
            url: "../Button/GetMyButton",
            async: false,
            data: { MenuID: MenuID, Num: Num },
            dataType: "json",
            success: function (result) {
                for (var i = 0, l = result.Data.length; i < l; i++) {
                    var o = result.Data[i];
                    if (i > 0) {
                        tool[tool.length] = "-";
                    }
                    tool += '<a href="#" class="easyui-linkbutton"  data-options="iconCls:\'' + o.IconPath + '\',plain:true"  onclick="javascript:' + o.Code + '();">' + o.Name + '</a>';
                }
            },
            error: function () {
                Easy.centerShow("系统消息", "按钮信息加载失败", 3000);
            }
        });
        return tool;
    }

    //加载单独按钮
    Easy.loadAloneToolbar = function (MenuID, Num) {
        var str = "";
        $.ajax({
            type: "post",
            url: "../Button/GetMyButton",
            async: false,
            data: { MenuID: MenuID, Num: Num },
            dataType: "json",
            success: function (result) {
                for (var i = 0, l = result.Data.length; i < l; i++) {
                    var o = result.Data[i];
                    str += "&nbsp;<a href=\"javascript:void(0)\" class=\"easyui-linkbutton\" data-options=\"iconCls:'" + o.IconPath + "'\" onclick=\"" + o.Code + "()\">" + o.Name + "</a>";
                }
            },
            error: function () {
                Easy.centerShow("系统消息", "按钮信息加载失败", 3000);
            }
        });
        return str;
    }

    //获取Url参数值
    Easy.GetQueryString = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }

    //验证是否选择行(单)
    Easy.checkRow = function (rows, message) {
        if (rows.length === 0) {//没有选择
            Easy.centerShow("系统消息", "请选择" + message, 3000);
            return false;
        }
        if (rows.length != 1) {//只能选择一个
            Easy.centerShow("系统消息", "只能选择一个" + message, 3000);
            return false;
        }
        return true;
    }
    Easy.checkTreeGridRow = function (rows, message) {
        if (rows === null) {//没有选择
            Easy.centerShow("系统消息", "请选择" + message, 3000);
            return false;
        }
        return true;
    }

    //验证是否选择行(多)
    Easy.checkRows = function (rows, message) {
        if (rows.length === 0) {//没有选择
            Easy.centerShow("系统消息", "请选择" + message, 3000);
            return false;
        }
        return true;
    }

    //验证是否选择树(单)
    Easy.checkNode = function (nodes, message) {
        if (nodes === null || "" + nodes === "") {//没有选择
            Easy.centerShow("系统消息", "请选择" + message, 3000);
            return false;
        }
        return true;
    }

    //更新值（修改状态）
    Easy.bindUpdateValue = function (confirmstr, value, url, rowsId, type, id) {
        $.messager.confirm("", "确定要" + confirmstr + "吗？", function (c) {
            if (c) {
                $.ajax({
                    type: "post",
                    url: url,
                    async: true,
                    data: { ID: rowsId, Value: value },
                    dataType: "json",
                    success: function (result) {
                        if (result.IsError === false) {
                            Easy.centerShow("系统消息", confirmstr + "成功", 3000);
                            if (type === "1") {
                                $(id).datagrid("reload");//刷新表格
                            }
                            if (type === "2") {
                                $(id).tree("reload");//刷新树
                            }
                            if (type === "3") {
                                $(id).treegrid("reload");//刷新树
                            }
                        }
                        else {
                            Easy.centerShow("系统消息", result.Message, 3000);
                        }
                    },
                    error: function () {
                        Easy.centerShow("系统消息", "操作失败", 3000);
                    }
                });
            }
        });
    }

    //批量更新值（修改状态）
    Easy.bindUpdateValues = function (confirmstr, value, url, rowsIdStr, type, id) {
        $.messager.confirm("", "确定要" + confirmstr + "吗？", function (c) {
            if (c) {
                $.ajax({
                    type: "post",
                    url: url,
                    async: true,
                    data: { IDStr: rowsIdStr, Value: value },
                    dataType: "json",
                    success: function (result) {
                        if (result.IsError === false) {
                            Easy.centerShow("系统消息", confirmstr + "成功", 3000);
                            if (type === "1") {
                                $(id).datagrid("reload");//刷新表格
                            }
                            if (type === "2") {
                                $(id).tree("reload");//刷新树
                            }
                            if (type === "3") {
                                $(id).treegrid("reload");//刷新树
                            }
                        }
                        else {
                            Easy.centerShow("系统消息", result.Message, 3000);
                        }
                    },
                    error: function () {
                        Easy.centerShow("系统消息", "操作失败", 3000);
                    }
                });
            }
        });
    }
    Easy.bindSelectInfoAsync = function (url, paras) {
        var result = "";
        $.ajax({
            type: "post",
            url: url,
            async: false,
            data: paras,
            dataType: "json",
            success: function (data) {
                result = data;
            },
            error: function () {
                Easy.centerShow("系统消息", "信息加载失败", 3000);
            }

        });
        return result;
    }
    //绑定查询信息的方法
    Easy.bindSelectInfo = function (url, id) {
        var result = "";
        $.ajax({
            type: "post",
            url: url,
            async: false,
            data: { ID: id },
            dataType: "json",
            success: function (data) {
                result = data;
            },
            error: function () {
                Easy.centerShow("系统消息", "信息加载失败", 3000);
            }
        });
        return result;
    }
    Easy.bindSelectInfomation = function (url, paras) {
        var result = "";
        $.ajax({
            type: "post",
            url: url,
            async: false,
            data: paras,
            dataType: "json",
            success: function (data) {
                result = data;
            },
            error: function () {
                Easy.centerShow("系统消息", "信息加载失败", 3000);
            }
        });
        return result;
    }
    //绑定开关高级搜索事件
    Easy.bindOpenCloseSearchBoxEvent = function (height) {
        $("#openSearchBox").click(function () {
            $("#infoMain").layout("panel", "north").panel("resize", { height: height });
            $("#infoMain").layout("resize");
            $("#openSearchBox").hide();
            $("#closeSearchBox").show();
        });
        $("#closeSearchBox").click(function () {
            $("#infoMain").layout("panel", "north").panel("resize", { height: 57 });
            $("#infoMain").layout("resize");
            $("#closeSearchBox").hide();
            $("#openSearchBox").show();
        });
    }

    //自定义提示事件
    Easy.bindCustomPromptEvent = function (id, showEvent, warnText) {
        $(id).tooltip({
            position: "right",
            content: warnText,
            showEvent: showEvent,
            onShow: function () {
                $(this).tooltip("tip").css({
                    borderColor: "#CC9933",
                    backgroundColor: "#FFFFCC"
                });
            }
        });
    }

    //自定义提示事件，用于表格
    Easy.bindCustomPromptToTableEvent = function (id) {
        $(id).tooltip({
            onShow: function () {
                $(this).tooltip("tip").css({
                    borderColor: "#CC9933",
                    backgroundColor: "#FFFFCC"
                });
            }
        });
    }

    //重置编辑时间格式事件
    Easy.bindResetTimeFormatEvent = function () {
        $.extend($.fn.datagrid.defaults.editors.datebox, {
            setValue: function (target, value) {
                $(target).datebox("setValue", Easy.bindSetTimeFormatEvent(value));//设置新值的日期格式
            }
        });
    }

    //设置时间格式
    Easy.bindSetTimeFormatEvent = function (date) {
        date = new Date(date);
        var y = date.getFullYear();
        var m = (date.getMonth() + 1) < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var d = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        var temp = y + "-" + m + "-" + d;
        if (temp === "1900-01-01") {
            return "";
        }
        else {
            return temp;
        }
    }

    //权限验证事件
    Easy.bindPowerValidationEvent = function (menuId, num, buttonCode) {
        var result = false;
        if (menuId === "" && num === "") {
            result = true;
        }
        else {
            $.ajax({
                type: "post",
                url: "../Power/GetValidatePower",
                async: false,
                data: { MenuID: menuId, Num: num, ButtonCode: buttonCode },
                dataType: "json",
                success: function (data) {
                    if (data.IsError === false) {
                        result = true;
                    }
                    else {
                        Easy.centerShow("系统消息", "没有权限", 3000);
                    }
                },
                error: function () {
                    Easy.centerShow("系统消息", "获取权限失败，请重新登录", 3000);
                }
            });
        }
        return result;
    }
    Easy.GetDateTimeNow = function () {
        var date = null;
        $.ajax({
            type: "post",
            url: "../Common/GetDateNow",
            async: false,
            data: {},
            dataType: "json",
            success: function (result) {
                if (result.iserror != false) {
                    date = result.Message;
                }
            },
            error: function () {
                Easy.centerShow("系统消息", "系统错误，请联系管理员！", 3000);
            }
        });
        return date;
    }
    Easy.bindPowerValidationEventUsetoHidePower = function (menuId, num, buttonCode) {
        var result = false;
        if (menuId === "" && num === "") {
            result = true;
        }
        else {
            $.ajax({
                type: "post",
                url: "../Power/GetValidatePower",
                async: false,
                data: { MenuID: menuId, Num: num, ButtonCode: buttonCode },
                dataType: "json",
                success: function (data) {
                    if (data.IsError === false) {
                        result = true;
                    }
                },
                error: function () {
                    Easy.centerShow("系统消息", "获取权限失败，请重新登录", 3000);
                }
            });
        }
        return result;
    }
    //证书打印模板
    Easy.AddProvePrintContent = function (parentDeptName, voucherNum, noteNum, deptName, studentName, iDCard, feeMode, feeTime, itemName, shouldMoney, feeContent, remark, classText, teacher, feeer) {
        var printStr = "";
        printStr += "<table style=\"width: 380px;\">";
        printStr += "<tr>";
        printStr += "<td style=\"font-size: 18px; font-weight: bold; text-align: center;\">" + parentDeptName + "证书费收据</td>";
        printStr += "</tr>";
        printStr += "</table>";
        printStr += "<table style=\"width: 380px; font-size: 14px;\">";
        printStr += "<tr>";
        printStr += "<td style=\"width: 60px; text-align: right;\">凭证号:</td>";
        printStr += "<td style=\"width: 160px;\">" + voucherNum + "</td>";
        printStr += "<td style=\"width: 30px;\"></td>";
        printStr += "<td style=\"width: 60px; text-align: right;\">票据号:</td>";
        printStr += "<td style=\"width: 70px;\">" + noteNum + "</td>";
        printStr += "</tr>";
        printStr += "</table>";
        printStr += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 380px; border-collapse: collapse; font-size: 14px; text-align: center; line-height: 25px;\">";
        printStr += "<tr>";
        printStr += "<td style=\"border: 1px solid #000000;\">校区:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align:left; padding-left:5px;\" colspan=\"3\">" + deptName + "</td>";
        printStr += "</tr>";
        printStr += "<tr>";
        printStr += "<td style=\"border: 1px solid #000000; width:70px;\">交费人:</td>";
        printStr += "<td style=\"border: 1px solid #000000; width:90px; text-align: left; padding-left: 5px;\">" + studentName + "</td>";
        printStr += "<td style=\"border: 1px solid #000000; width:70px;\">证件号码:</td>";
        printStr += "<td style=\"border: 1px solid #000000; width:150px; text-align: left; padding-left: 5px;\">" + iDCard + "</td>";
        printStr += "</tr>";
        printStr += "<tr>";
        printStr += "<td style=\"border: 1px solid #000000;\">证书名称:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\" colspan=\"3\">" + itemName + "</td>";
        printStr += "</tr>";
        printStr += "<tr>";
        printStr += "<td style=\"border: 1px solid #000000;\">费用合计:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\">" + feeMode + "" + shouldMoney + "</td>";
        printStr += "<td style=\"border: 1px solid #000000;\">大写:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\">" + Easy.convertCurrency(shouldMoney) + "</td>";
        printStr += "</tr>";

        var str = "<div>";
        var num = (feeContent.length - feeContent.replace(/<div>/g, "").length) / str.length;
        var rowspan = num / 2 + num % 2;
        for (var i = 0; i < num; i++) {
            if ((i + 1) % 2 === 1) {
                if (i === 0) {
                    feeContent = "<tr><td style=\"border: 1px solid #000000;\" rowspan=\"" + rowspan + "\">收费项目:</td>" + feeContent;
                    feeContent = feeContent.replace("<div>", "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\" colspan=\"2\">");
                }
                else {
                    feeContent = feeContent.replace("<div>", "<tr><td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\" colspan=\"2\">");
                }
                feeContent = feeContent.replace("</div>", "</td>");
            }
            else {
                feeContent = feeContent.replace("<div>", "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\">");
                feeContent = feeContent.replace("</div>", "</td></tr>");
            }
        }
        if (num % 2 === 1) {
            feeContent += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\"></td></tr>";
        }
        //printStr += feeContent;
        printStr += "<tr>";
        printStr += "<td style=\"border: 1px solid #000000;\">任课教师:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\">" + teacher + "</td>";
        printStr += "<td style=\"border: 1px solid #000000;\">就读班级:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\">" + classText + "</td>";
        printStr += "</tr>";
        printStr += "<tr>";
        printStr += "<td style=\"border: 1px solid #000000;\">收费人:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\">" + feeer + "</td>";
        printStr += "<td style=\"border: 1px solid #000000;\">收费时间:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\">" + feeTime + "</td>";
        printStr += "</tr>";
        printStr += "<tr>";
        printStr += "<td style=\"border: 1px solid #000000;\">备注:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px; width: 310px;\" colspan=\"3\">" + remark + "</td>";
        printStr += "</tr>";
        printStr += "</table>";
        return printStr;
    }
    //批量打印证书
    Easy.AddProvePrintMoreContent = function (model, noteNum) {
        var printStr = "";
        printStr += "<table style=\"width: 380px;\">";
        printStr += "<tr>";
        printStr += "<td style=\"font-size: 18px; font-weight: bold; text-align: center;\">" + model.DeptName + "证书费收据</td>";
        printStr += "</tr>";
        printStr += "</table>";
        printStr += "<table style=\"width: 380px; font-size: 14px;\">";
        printStr += "<tr>";
        printStr += "<td style=\"width: 60px; text-align: right;\"></td>";
        printStr += "<td style=\"width: 160px;\"></td>";
        printStr += "<td style=\"width: 30px;\"></td>";
        printStr += "<td style=\"width: 60px; text-align: right;\">票据号:</td>";
        printStr += "<td style=\"width: 70px;\">" + noteNum + "</td>";
        printStr += "</tr>";
        printStr += "</table>";
        printStr += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 380px; border-collapse: collapse; font-size: 14px; text-align: center; line-height: 25px;\">";
        printStr += "<tr>";
        printStr += "<td style=\"border: 1px solid #000000;\">校区:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align:left; padding-left:5px;\" colspan=\"3\">" + model.SchoolName + "</td>";
        printStr += "</tr>";
        printStr += "<tr>";
        printStr += "<td style=\"border: 1px solid #000000;\">就读专业:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px; width: 310px;\" colspan=\"3\">" + model.ProName + "</td>";
        printStr += "</tr>";
        printStr += "<tr>";
        printStr += "<td style=\"border: 1px solid #000000;\">就读班级:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px; width: 310px;\" colspan=\"3\">" + model.ClassName + "</td>";
        printStr += "</tr>";

        printStr += "<tr>";
        printStr += "<td style=\"border: 1px solid #000000;\">证书名称:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px; width: 310px;\" colspan=\"3\">" + model.ProveName + "</td>";
        printStr += "</tr>";


        var str = "<div>";
        var num = (model.FeeContent.length - model.FeeContent.replace(/<div>/g, "").length) / str.length;
        var rowspan = num / 2 + num % 2;
        for (var i = 0; i < num; i++) {
            if ((i + 1) % 2 === 1) {
                if (i === 0) {
                    model.FeeContent = "<tr><td style=\"border: 1px solid #000000;\" rowspan=\"" + rowspan + "\">收费项目:</td>" + model.FeeContent;
                    model.FeeContent = model.FeeContent.replace("<div>", "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\" colspan=\"2\">");
                }
                else {
                    model.FeeContent = model.FeeContent.replace("<div>", "<tr><td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\" colspan=\"2\">");
                }
                model.FeeContent = model.FeeContent.replace("</div>", "</td>");
            }
            else {
                model.FeeContent = model.FeeContent.replace("<div>", "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\">");
                model.FeeContent = model.FeeContent.replace("</div>", "</td></tr>");
            }
        }
        if (num % 2 === 1) {
            model.FeeContent += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\"></td></tr>";
        }
        //printStr += model.FeeContent;
        printStr += "<tr>";
        printStr += "<td style=\"border: 1px solid #000000;  width:70px;\">费用合计:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px; width:90px;\">" + model.FeeMode + "" + model.ShouldMoney + "</td>";
        printStr += "<td style=\"border: 1px solid #000000; width:70px;\">大写:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px; width:150px;\">" + Easy.convertCurrency(model.ShouldMoney) + "</td>";
        printStr += "</tr>";

        printStr += "<tr>";
        printStr += "<td style=\"border: 1px solid #000000;\">缴费人:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px; width: 310px;\" colspan=\"3\">" + model.FeeUser.substring(0, model.FeeUser.length - 1) + "</td>";
        printStr += "</tr>";
        printStr += "<tr>";
        printStr += "<td style=\"border: 1px solid #000000;\">收费人:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\">" + model.Creater + "</td>";
        printStr += "<td style=\"border: 1px solid #000000;\">收费时间:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\">" + model.FeeTime + "</td>";
        printStr += "</tr>";
        return printStr;
    }

    //杂费打印模板
    Easy.AddIncPrintContent = function (parentDeptName, voucherNum, noteNum, deptName, studentName, iDCard, feeMode, feeTime, itemName, shouldMoney, remark, feeer) {
        var printStr = "";
        printStr += "<table style=\"width: 380px;\">";
        printStr += "<tr>";
        printStr += "<td style=\"font-size: 18px; font-weight: bold; text-align: center;\">" + parentDeptName + "杂费收据</td>";
        printStr += "</tr>";
        printStr += "</table>";
        printStr += "<table style=\"width: 380px; font-size: 14px;\">";
        printStr += "<tr>";
        printStr += "<td style=\"width: 60px; text-align: right;\">凭证号:</td>";
        printStr += "<td style=\"width: 140px;\">" + voucherNum + "</td>";
        printStr += "<td style=\"width: 30px;\"></td>";
        printStr += "<td style=\"width: 60px; text-align: right;\">票据号:</td>";
        printStr += "<td style=\"width: 90px;\">" + noteNum + "</td>";
        printStr += "</tr>";
        printStr += "</table>";
        printStr += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 380px; border-collapse: collapse; font-size: 14px; text-align: center; line-height: 25px;\">";
        printStr += "<tr>";
        printStr += "<td style=\"border: 1px solid #000000;\">收费单位:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align:left; padding-left:5px;\" colspan=\"3\">" + deptName + "</td>";
        printStr += "</tr>";
        printStr += "<tr>";
        printStr += "<td style=\"border: 1px solid #000000; width:70px;\">交费人:</td>";
        printStr += "<td style=\"border: 1px solid #000000; width:70px; text-align: left; padding-left: 5px;\">" + studentName + "</td>";
        printStr += "<td style=\"border: 1px solid #000000; width:70px;\">证件号码:</td>";
        printStr += "<td style=\"border: 1px solid #000000; width:170px; text-align: left; padding-left: 5px;\">" + iDCard + "</td>";
        printStr += "</tr>";
        printStr += "<tr>";
        printStr += "<td style=\"border: 1px solid #000000;\">收费项目:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\" colspan=\"3\">" + itemName + "</td>";
        printStr += "</tr>";
        printStr += "<tr>";
        printStr += "<td style=\"border: 1px solid #000000;\">费用合计:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\">" + shouldMoney + "</td>";
        printStr += "<td style=\"border: 1px solid #000000;\">交费方式:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\">" + feeMode + "</td>";
        printStr += "</tr>";
        printStr += "<tr>";
        printStr += "<td style=\"border: 1px solid #000000;\">大写:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\" colspan=\"3\">" + Easy.convertCurrency(shouldMoney) + "</td>";
        printStr += "</tr>";
        printStr += "<tr>";
        printStr += "<td style=\"border: 1px solid #000000;\">收费人:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\">" + feeer + "</td>";
        printStr += "<td style=\"border: 1px solid #000000;\">收费时间:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\">" + feeTime + "</td>";
        printStr += "</tr>";
        printStr += "<tr>";
        printStr += "<td style=\"border: 1px solid #000000;\">备注:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px; width: 310px;\" colspan=\"3\">" + remark + "</td>";
        printStr += "</tr>";
        printStr += "</table>";
        return printStr;
    }

    //合并打印杂费
    Easy.iFeePrintMoreContent = function (model, noteNum) {
        var printStr = "";
        printStr += "<table style=\"width: 380px;\">";
        printStr += "<tr>";
        printStr += "<td style=\"font-size: 18px; font-weight: bold; text-align: center;\">" + model.DeptName + "杂费收据</td>";
        printStr += "</tr>";
        printStr += "</table>";
        printStr += "<table style=\"width: 380px; font-size: 14px;\">";
        printStr += "<tr>";
        printStr += "<td style=\"width: 60px; text-align: right;\"></td>";
        printStr += "<td style=\"width: 140px;\"></td>";
        printStr += "<td style=\"width: 30px;\"></td>";
        printStr += "<td style=\"width: 60px; text-align: right;\">票据号:</td>";
        printStr += "<td style=\"width: 90px;\">" + noteNum + "</td>";
        printStr += "</tr>";
        printStr += "</table>";
        printStr += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 380px; border-collapse: collapse; font-size: 14px; text-align: center; line-height: 25px;\">";
        printStr += "<tr>";
        printStr += "<td style=\"border: 1px solid #000000;\">收费单位:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align:left; padding-left:5px;\" colspan=\"3\">" + model.SchoolName + "</td>";
        printStr += "</tr>";
        printStr += "<td style=\"border: 1px solid #000000;\">收费项目:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align:left; padding-left:5px;\" colspan=\"3\">" + model.FeeContent + "</td>";
        printStr += "</tr>";
        printStr += "<tr>";
        printStr += "<td style=\"border: 1px solid #000000;  width:70px;\">费用合计:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px; width:90px;\">" + model.FeeMode + "" + model.ShouldMoney + "</td>";
        printStr += "<td style=\"border: 1px solid #000000; width:70px;\">大写:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px; width:150px;\">" + Easy.convertCurrency(model.ShouldMoney) + "</td>";
        printStr += "</tr>";

        printStr += "<tr>";
        printStr += "<td style=\"border: 1px solid #000000;\">缴费人:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px; width: 310px;\" colspan=\"3\">" + model.FeeUser.substring(0, model.FeeUser.length - 1) + "</td>";
        printStr += "</tr>";
        printStr += "<tr>";
        printStr += "<td style=\"border: 1px solid #000000;\">收费人:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\">" + model.Creater + "</td>";
        printStr += "<td style=\"border: 1px solid #000000;\">收费时间:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\">" + model.FeeTime + "</td>";
        printStr += "</tr>";
        return printStr;
    }
    //同人缴费打印
    Easy.iFeePrintBySameMoreContent = function (model, noteNum) {
        var printStr = "";
        printStr += "<table style=\"width: 380px;\">";
        printStr += "<tr>";
        printStr += "<td style=\"font-size: 18px; font-weight: bold; text-align: center;\">" + model.DeptName + "杂费收据</td>";
        printStr += "</tr>";
        printStr += "</table>";
        printStr += "<table style=\"width: 380px; font-size: 14px;\">";
        printStr += "<tr>";
        printStr += "<td style=\"width: 60px; text-align: right;\"></td>";
        printStr += "<td style=\"width: 140px;\"></td>";
        printStr += "<td style=\"width: 30px;\"></td>";
        printStr += "<td style=\"width: 60px; text-align: right;\">票据号:</td>";
        printStr += "<td style=\"width: 90px;\">" + noteNum + "</td>";
        printStr += "</tr>";
        printStr += "</table>";
        printStr += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 380px; border-collapse: collapse; font-size: 14px; text-align: center; line-height: 25px;\">";
        printStr += "<tr>";
        printStr += "<td style=\"border: 1px solid #000000;\">收费单位:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align:left; padding-left:5px;\" colspan=\"3\">" + model.SchoolName + "</td>";
        printStr += "<tr>";
        printStr += "<td style=\"border: 1px solid #000000;\">缴费人:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px; width: 310px;\" colspan=\"3\">" + model.FeeUser + " " + model.IDCard + "</td>";
        printStr += "</tr>";
        printStr += "</tr>";
        printStr += "<td style=\"border: 1px solid #000000;\">收费项目:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align:left; padding-left:5px;\" colspan=\"3\">" + model.FeeContent + "</td>";
        printStr += "</tr>";

        printStr += "<tr>";
        printStr += "<td style=\"border: 1px solid #000000;  width:70px;\">费用合计:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align:left; padding-left:5px;\" colspan=\"3\"> " + model.ShouldMoney + "</td>";
        printStr += "</tr>";
        printStr += "</tr>";
        printStr += "<td style=\"border: 1px solid #000000;\">大写:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align:left; padding-left:5px;\" colspan=\"3\">" + Easy.convertCurrency(model.ShouldMoney) + "</td>";
        printStr += "</tr>";
        printStr += "<tr>";
        printStr += "<td style=\"border: 1px solid #000000;\">收费人:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\">" + model.Creater + "</td>";
        printStr += "<td style=\"border: 1px solid #000000;\">收费时间:</td>";
        printStr += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\">" + model.FeeTime + "</td>";
        printStr += "</tr>";
        return printStr;
    }


    //打印学费收据
    Easy.AddStuFeePrintContent = function (data, noteNum) {

        var printString = "";
        printString += "<table style=\"width: 380px;\">";
        printString += "<tr>";
        printString += "<td style=\"font-size: 18px; font-weight: bold; text-align: center;\">" + data.DeptName + "学费收据</td>";
        printString += "</tr>";
        printString += "</table>";
        printString += "<table style=\"width: 380px; font-size: 14px;\">";
        printString += "<tr>";
        printString += "<td style=\"width: 60px; text-align: right;\">凭证号:</td>";
        printString += "<td style=\"width: 160px;\">" + data.VoucherNum + "</td>";
        printString += "<td style=\"width: 20px;\"></td>";
        printString += "<td style=\"width: 60px; text-align: right;\">票据号:</td>";
        printString += "<td style=\"width: 80px;\">" + noteNum + "</td>";
        printString += "</tr>";
        printString += "</table>";
        printString += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 380px; border-collapse: collapse; font-size: 14px; text-align: center; line-height: 25px;\">";
        printString += "<tr>";
        printString += "<td style=\"border: 1px solid #000000;\">收费主体:</td>";
        printString += "<td style=\"border: 1px solid #000000; text-align:left; padding-left:5px;\" colspan=\"3\">" + data.SchoolName + "</td>";
        printString += "</tr>";
        printString += "<tr>";
        printString += "<td style=\"border: 1px solid #000000;\">就读校区:</td>";
        printString += "<td style=\"border: 1px solid #000000; text-align:left; padding-left:5px;\" colspan=\"3\">" + data.DeptAreaName + "</td>";
        printString += "</tr>";
        printString += "<tr>";
        printString += "<td style=\"border: 1px solid #000000;\">就读专业:</td>";
        printString += "<td style=\"border: 1px solid #000000; text-align:left; padding-left:5px;\" colspan=\"3\">" + data.ProfeessName + "</td>";
        printString += "</tr>";
        printString += "<tr>";
        printString += "<td style=\"border: 1px solid #000000; width:70px;\">缴费次数:</td>";
        printString += "<td style=\"border: 1px solid #000000; width:90px; text-align: left; padding-left: 5px;\">" + data.NumName + "</td>";
        printString += "<td style=\"border: 1px solid #000000; width:70px;\">学号:</td>";
        printString += "<td style=\"border: 1px solid #000000; width:150px; text-align: left; padding-left: 5px;\">" + data.EnrollNum + "</td>";
        printString += "</tr>";
        printString += "<tr>";
        printString += "<td style=\"border: 1px solid #000000; width:70px;\">交费人:</td>";
        printString += "<td style=\"border: 1px solid #000000; width:90px; text-align: left; padding-left: 5px;\">" + data.StuName + "</td>";
        printString += "<td style=\"border: 1px solid #000000; width:70px;\">证件号码:</td>";
        printString += "<td style=\"border: 1px solid #000000; width:150px; text-align: left; padding-left: 5px;\">" + data.IDCard + "</td>";
        printString += "</tr>";
        //printString += "<tr>";
        //printString += "<td style=\"border: 1px solid #000000;\">报读层次:</td>";
        //printString += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\" colspan=\"3\">" + data.EnrollLevel + "</td>";
        //printString += "</tr>";
        var str = "<div>";
        var num = (data.sfeeContent.length - data.sfeeContent.replace(/<div>/g, "").length) / str.length;
        var rowspan = num / 2 + num % 2;
        for (var i = 0; i < num; i++) {
            if ((i + 1) % 2 === 1) {
                if (i === 0) {
                    data.sfeeContent = "<tr><td style=\"border: 1px solid #000000;\" rowspan=\"" + rowspan + "\">缴费内容:</td>" + data.sfeeContent;
                    data.sfeeContent = data.sfeeContent.replace("<div>", "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\" colspan=\"2\">");
                }
                else {
                    data.sfeeContent = data.sfeeContent.replace("<div>", "<tr><td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\" colspan=\"2\">");
                }
                data.sfeeContent = data.sfeeContent.replace("</div>", "</td>");
            }
            else {
                data.sfeeContent = data.sfeeContent.replace("<div>", "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\">");
                data.sfeeContent = data.sfeeContent.replace("</div>", "</td></tr>");
            }
        }
        if (num % 2 === 1) {
            data.sfeeContent += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\"></td></tr>";
        }
        printString += data.sfeeContent;
        if (data.GiveName != "") {
            var giveName = data.GiveName.replace(/<div>/g, "").replace(/<\/div>/g, "");
            giveName = giveName.substring(0, giveName.length - 1);
            printString += "<tr>";
            printString += "<td style=\"border: 1px solid #000000;\">配品:</td>";
            printString += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px; \" colspan=\"3\">" + giveName + "</td>";
            printString += "</tr>";
        }
        printString += "<tr>";
        printString += "<td style=\"border: 1px solid #000000;\">费用合计:</td>";
        printString += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\">" + data.PaidMoney + "</td>";
        printString += "<td style=\"border: 1px solid #000000;\">缴费方式:</td>";
        printString += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px; \" >" + data.FeeMode + "</td>";
        printString += "</tr>";
        printString += "<tr>";
        printString += "<td style=\"border: 1px solid #000000;\">大写:</td>";
        printString += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px; width: 310px;\" colspan=\"3\">" + Easy.convertCurrency(data.PaidMoney) + "</td>";
        printString += "</tr>";

        printString += "<tr>";
        printString += "<td style=\"border: 1px solid #000000;\">收费人:</td>";
        printString += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\">" + data.UserName + "</td>";
        printString += "<td style=\"border: 1px solid #000000;\">收费时间:</td>";
        printString += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\">" + data.FeeTime + "</td>";
        printString += "</tr>";

        printString += "<tr>";
        printString += "<td style=\"border: 1px solid #000000;\">备注:</td>";
        printString += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px; width: 310px;\" colspan=\"3\">" + data.PaidRemark + data.OffsetRemark + data.DiscountRemark + data.ByOffsetRemark + data.RefundRemark + data.Explain + "</td>";
        printString += "</tr>";
        printString += "</table>";
        return printString;
    };
    //打印合计
    Easy.AddStuFeePrintTotalContent = function (data, noteNum) {
        var printString = "";
        printString += "<table style=\"width: 380px;\">";
        printString += "<tr>";
        printString += "<td style=\"font-size: 18px; font-weight: bold; text-align: center;\">" + data.DeptName + "学费收据</td>";
        printString += "</tr>";
        printString += "</table>";
        printString += "<table style=\"width: 380px; font-size: 14px;\">";
        printString += "<tr>";
        printString += "<td style=\"width: 60px; text-align: right;\">凭证号:</td>";
        printString += "<td style=\"width: 160px;\">" + data.VoucherNum + "</td>";
        printString += "<td style=\"width: 20px;\"></td>";
        printString += "<td style=\"width: 60px; text-align: right;\">票据号:</td>";
        printString += "<td style=\"width: 80px;\">" + noteNum + "</td>";
        printString += "</tr>";
        printString += "</table>";
        printString += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 380px; border-collapse: collapse; font-size: 14px; text-align: center; line-height: 25px;\">";
        printString += "<tr>";
        printString += "<td style=\"border: 1px solid #000000;\">收费主体:</td>";
        printString += "<td style=\"border: 1px solid #000000; text-align:left; padding-left:5px;\" colspan=\"3\">" + data.SchoolName + "</td>";
        printString += "</tr>";
        printString += "<tr>";
        printString += "<td style=\"border: 1px solid #000000;\">就读校区:</td>";
        printString += "<td style=\"border: 1px solid #000000; text-align:left; padding-left:5px;\" colspan=\"3\">" + data.DeptAreaName + "</td>";
        printString += "</tr>";
        printString += "<tr>";
        printString += "<td style=\"border: 1px solid #000000;\">就读专业:</td>";
        printString += "<td style=\"border: 1px solid #000000; text-align:left; padding-left:5px;\" colspan=\"3\">" + data.ProfeessName + "</td>";
        printString += "</tr>";
        printString += "<tr>";
        //printString += "<td style=\"border: 1px solid #000000; width:70px;\">报读层次:</td>";
        //printString += "<td style=\"border: 1px solid #000000; width:90px; text-align: left; padding-left: 5px;\">" + data.EnrollLevel + "</td>";
        printString += "<td style=\"border: 1px solid #000000; width:70px;\">学号:</td>";
        printString += "<td style=\"border: 1px solid #000000; width:150px; text-align: left; padding-left: 5px;\" colspan=\"3\">" + data.EnrollNum + "</td>";
        printString += "</tr>";
        printString += "<tr>";
        printString += "<td style=\"border: 1px solid #000000; width:70px;\">交费人:</td>";
        printString += "<td style=\"border: 1px solid #000000; width:90px; text-align: left; padding-left: 5px;\">" + data.StuName + "</td>";
        printString += "<td style=\"border: 1px solid #000000; width:70px;\">证件号码:</td>";
        printString += "<td style=\"border: 1px solid #000000; width:150px; text-align: left; padding-left: 5px;\">" + data.IDCard + "</td>";
        printString += "</tr>";

        if (data.GiveName != "") {
            var giveName = data.GiveName.replace(/<div>/g, "").replace(/<\/div>/g, "");
            giveName = giveName.substring(0, giveName.length - 1);
            printString += "<tr>";
            printString += "<td style=\"border: 1px solid #000000;\">配品:</td>";
            printString += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px; \" colspan=\"3\">" + giveName + "</td>";
            printString += "</tr>";
        }
        printString += "<tr>";
        printString += "<td style=\"border: 1px solid #000000;\">费用合计:</td>";
        printString += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\">" + data.PaidMoney + "</td>";
        printString += "<td style=\"border: 1px solid #000000;\">缴费方式:</td>";
        printString += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px; \" >" + data.FeeMode + "</td>";
        printString += "</tr>";
        printString += "<tr>";
        printString += "<td style=\"border: 1px solid #000000;\">大写:</td>";
        printString += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px; width: 310px;\" colspan=\"3\">" + Easy.convertCurrency(data.PaidMoney) + "</td>";
        printString += "</tr>";

        printString += "<tr>";
        printString += "<td style=\"border: 1px solid #000000;\">收费人:</td>";
        printString += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\">" + data.UserName + "</td>";
        printString += "<td style=\"border: 1px solid #000000;\">收费时间:</td>";
        printString += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px;\">" + data.FeeTime + "</td>";
        printString += "</tr>";

        printString += "<tr>";
        printString += "<td style=\"border: 1px solid #000000;\">备注:</td>";
        printString += "<td style=\"border: 1px solid #000000; text-align: left; padding-left: 5px; width: 310px;\" colspan=\"3\">" + data.PaidRemark + data.OffsetRemark + data.DiscountRemark + data.ByOffsetRemark + data.RefundRemark + data.Explain + "</td>";
        printString += "</tr>";
        printString += "</table>";
        return printString;
    };

    Easy.convertCurrency = function (currencyDigits) {
        // Constants:
        var MAXIMUM_NUMBER = 99999999999.99;
        // Predefine the radix characters and currency symbols for output:
        var CN_ZERO = "零";
        var CN_ONE = "壹";
        var CN_TWO = "贰";
        var CN_THREE = "叁";
        var CN_FOUR = "肆";
        var CN_FIVE = "伍";
        var CN_SIX = "陆";
        var CN_SEVEN = "柒";
        var CN_EIGHT = "捌";
        var CN_NINE = "玖";
        var CN_TEN = "拾";
        var CN_HUNDRED = "佰";
        var CN_THOUSAND = "仟";
        var CN_TEN_THOUSAND = "万";
        var CN_HUNDRED_MILLION = "亿";
        //var CN_SYMBOL = "人民币";
        var CN_SYMBOL = "";
        var CN_DOLLAR = "元";
        var CN_TEN_CENT = "角";
        var CN_CENT = "分";
        var CN_INTEGER = "整";
        // Variables:
        var integral; // Represent integral part of digit number.
        var decimal; // Represent decimal part of digit number.
        var outputCharacters; // The output result.
        var parts;
        var digits, radices, bigRadices, decimals;
        var zeroCount;
        var i, p, d;
        var quotient, modulus;
        // Validate input string:
        currencyDigits = currencyDigits.toString();
        if (currencyDigits == "") {
            return "还没有输入数字！";
        }
        if (currencyDigits.match(/[^,.\d]/) != null) {
            return "请输入有效数字!";
        }
        if ((currencyDigits).match(/^((\d{1,3}(,\d{3})*(.((\d{3},)*\d{1,3}))?)|(\d+(.\d+)?))$/) == null) {
            return "请输入有效格式数字！";
        }
        // Normalize the format of input digits:
        currencyDigits = currencyDigits.replace(/,/g, ""); // Remove comma delimiters.
        currencyDigits = currencyDigits.replace(/^0+/, ""); // Trim zeros at the beginning.
        // Assert the number is not greater than the maximum number.
        if (Number(currencyDigits) > MAXIMUM_NUMBER) {
            return "您输入的数字太大了!";
        }
        // Process the coversion from currency digits to characters:
        // Separate integral and decimal parts before processing coversion:
        parts = currencyDigits.split(".");
        if (parts.length > 1) {
            integral = parts[0];
            decimal = parts[1];
            // Cut down redundant decimal digits that are after the second.
            decimal = decimal.substr(0, 2);
            if (decimal == "00") {
                decimal = "";
            }
        }
        else {
            integral = parts[0];
            decimal = "";
        }
        // Prepare the characters corresponding to the digits:
        digits = new Array(CN_ZERO, CN_ONE, CN_TWO, CN_THREE, CN_FOUR, CN_FIVE, CN_SIX, CN_SEVEN, CN_EIGHT, CN_NINE);
        radices = new Array("", CN_TEN, CN_HUNDRED, CN_THOUSAND);
        bigRadices = new Array("", CN_TEN_THOUSAND, CN_HUNDRED_MILLION);
        decimals = new Array(CN_TEN_CENT, CN_CENT);
        // Start processing:
        outputCharacters = "";
        // Process integral part if it is larger than 0:
        if (Number(integral) > 0) {
            zeroCount = 0;
            for (i = 0; i < integral.length; i++) {
                p = integral.length - i - 1;
                d = integral.substr(i, 1);
                quotient = p / 4;
                modulus = p % 4;
                if (d == "0") {
                    zeroCount++;
                }
                else {
                    if (zeroCount > 0) {
                        outputCharacters += digits[0];
                    }
                    zeroCount = 0;
                    outputCharacters += digits[Number(d)] + radices[modulus];
                }
                if (modulus == 0 && zeroCount < 4) {
                    outputCharacters += bigRadices[quotient];
                }
            }
            outputCharacters += CN_DOLLAR;
        }
        // Process decimal part if there is:
        if (decimal != "") {
            for (i = 0; i < decimal.length; i++) {
                d = decimal.substr(i, 1);
                if (d != "0") {
                    outputCharacters += digits[Number(d)] + decimals[i];
                }
            }
        }
        // Confirm and return the final output string:
        if (outputCharacters == "") {
            outputCharacters = CN_ZERO + CN_DOLLAR;
        }
        if (decimal == "") {
            outputCharacters += CN_INTEGER;
        }
        outputCharacters = CN_SYMBOL + outputCharacters;
        return outputCharacters;
    }
    Easy.bindSaveUploadFile = function (dialog, form, url, btn, type, loadId, menuId, num, power) {
        $(btn).click(function () {
            if ($(btn).linkbutton("options").disabled === false) {//按钮没有禁用才能执行（easyui特殊处理）
                var buttonCode = power;
                if (Easy.bindPowerValidationEvent(menuId, num, buttonCode)) {
                    $(form).form("submit", {
                        url: url,
                        onSubmit: function () {
                            var validate = $(form).form("validate");//验证
                            if (validate) {
                                $(btn).linkbutton("disable");//禁用按钮
                                $.messager.progress({ title: "提示", text: "保存中,请稍后..." });
                            }
                            return validate;
                        },
                        success: function (result) {
                            result = JSON.parse(result);
                            if (result.IsError === false) {
                                var data = JSON.parse(result.Message);
                                $("#Mesg").html(data.Mesg);
                                if (data.Url != "") {
                                    $("#hideTR").show();
                                    $("#BtnExport").attr('href', data.Url);
                                } else {
                                    $("#hideTR").hide();
                                    $("#BtnExport").attr('href', 'javascript:void(0)');
                                }
                                $(form).form("reset");
                                $(btn).linkbutton("enable");
                                Easy.centerShow("系统消息", data.Tip, 3000);
                                $(loadId).datagrid('reload');

                            }
                            else {
                                $(btn).linkbutton("enable");
                                Easy.centerShow("系统消息", result.Message, 3000);
                            }
                            $.messager.progress('close');

                        }

                    });
                }
            }
        });
    }

    Easy.UpLoadFile = function () {
        $("#file_upload").uploadify({
            'debug': true,
            'auto': true,
            'buttonImage': '../Content/Images/uploadpic.png',
            'formData': {
                'folder': '../ELoad/File'
            },
            'swf': "../Content/uploadify/uploadify.swf",
            'queueID': 'uploadfileQueue',
            'uploader': '../Upload/UploadList',
            'multi': false,
            'width': '80',
            'height': '20',
            'overrideEvents': ['onDialogClose'],
            'fileTypeDesc': '请选择.xls',
            'fileTypeExts': '*.xls',
            'fileSizeLimit': '6MB',
            'removeTimeout': 1,
            'onSelectError': function (file, errorCode, errorMsg) {
                switch (errorCode) {
                    case -100:
                        $("#Mesg").html("上传的文件数量已经超出系统限制！");
                        break;
                    case -110:
                        $("#Mesg").html("文件 [" + file.name + "] 大小超出系统限制的！");
                        break;
                    case -120:
                        $("#Mesg").html("文件 [" + file.name + "] 大小异常！");
                        break;
                    case -130:
                        $("#Mesg").html("文件 [" + file.name + "] 类型不正确！");
                        break;
                }
            }, 'onFallback': function () {
                $("#Mesg").html("您未安装FLASH控件，无法上传图片！请安装FLASH控件后再试。");
            },
            'onUploadSuccess': function (file, data, response) {
                //$("#Mesg").html("文件 [" + file.name + "] 可以上传！");
                $("#filePath").textbox("setValue", data);
            }
        });
    }
    // 导出(列表id，导出文件名)
    Easy.DeriveFile = function (grid, file) {
        $.extend($.fn.datagrid.methods, {
            setColumnTitle: function (jq, option) {
                var title = "";
                if (option.field) {
                    jq.each(function () {
                        var $panel = $(this).datagrid("getPanel");
                        var $field = $('td[field=' + option.field + ']', $panel);
                        if ($field.length) {
                            var $span = $("span", $field).eq(0);
                            title = $span.html();
                        }
                    });
                }
                return title;
            }
        });
        var data = $(grid).datagrid("getData");
        data = JSON.stringify(data);

        var opts = $(grid).datagrid("getColumnFields");
        for (var i = 0; i < opts.length; i++) {
            var title = $(grid).datagrid("setColumnTitle", { field: opts[i] });
            data = data.replace(opts[i], title);
        }
        data = data.replace("RowNumber", "序号");
        var filePath = file;
        $.ajax({
            type: "post",
            url: "../Derive/DeriveList",
            async: false,
            data: { FilePath: filePath, Data: data },
            dataType: "json",
            success: function (result) {
                if (result.IsError === false) {
                    location.href = "../Temp/" + filePath;
                }
                else
                    Easy.centerShow("系统消息", result.Message, 2000);
            }
        });
    }
    //导出grid数据（列表id，导出文件名）
    Easy.DeriveFileToGrid = function (grid, file) {
        var filePath = file;
        var html = "<style type=\"text/css\"> td {border: 1px solid #cccccc;}</style>" + $(grid).html();//加边框
        $.ajax({
            type: "post",
            url: "../Derive/DeriveData",
            async: false,
            data: { FilePath: filePath, Html: html },
            dataType: "json",
            success: function (result) {
                if (result.IsError === false) {
                    //Easy.centerShow("系统消息", "导出成功", 3000);
                    location.href = "../Temp/" + filePath;
                }
                else {
                    Easy.centerShow("系统消息", result.Message, 2000);
                }
            }
        });
    }

    $.extend($.fn.tree.methods, {
        /**  
         * 扩展easyui tree的搜索方法  
         * @param tree easyui tree的根DOM节点(UL节点)的jQuery对象  
         * @param searchText 检索的文本  
         * @param this-context easyui tree的tree对象  
         */
        search: function (jqTree, searchText) {
            //easyui tree的tree对象。可以通过tree.methodName(jqTree)方式调用easyui tree的方法  
            var tree = this;

            //获取所有的树节点  
            var nodeList = getAllNodes(jqTree, tree);

            //如果没有搜索条件，则展示所有树节点  
            searchText = $.trim(searchText).toUpperCase();
            if (searchText == "") {
                for (var i = 0; i < nodeList.length; i++) {
                    $(".tree-node-targeted", nodeList[i].target).removeClass("tree-node-targeted");
                    $(nodeList[i].target).show();
                }
                //展开已选择的节点（如果之前选择了）  
                var selectedNode = tree.getSelected(jqTree);
                if (selectedNode) {
                    tree.expandTo(jqTree, selectedNode.target);
                }
                return;
            }

            //搜索匹配的节点并高亮显示  
            var matchedNodeList = [];
            if (nodeList && nodeList.length > 0) {
                var node = null;
                for (var i = 0; i < nodeList.length; i++) {
                    node = nodeList[i];
                    if (isMatch(searchText, node.text)) {
                        matchedNodeList.push(node);
                    }
                }

                //隐藏所有节点  
                for (var i = 0; i < nodeList.length; i++) {
                    $(".tree-node-targeted", nodeList[i].target).removeClass("tree-node-targeted");
                    $(nodeList[i].target).hide();
                }

                //折叠所有节点  
                //tree.collapseAll(jqTree);

                //展示所有匹配的节点以及父节点              
                for (var i = 0; i < matchedNodeList.length; i++) {
                    showMatchedNode(jqTree, tree, matchedNodeList[i]);
                }
            }
        },

        /**  
         * 展示节点的子节点（子节点有可能在搜索的过程中被隐藏了）  
         * @param node easyui tree节点  
         */
        showChildren: function (jqTree, node) {
            //easyui tree的tree对象。可以通过tree.methodName(jqTree)方式调用easyui tree的方法  
            var tree = this;

            //展示子节点  
            if (!tree.isLeaf(jqTree, node.target)) {
                var children = tree.getChildren(jqTree, node.target);
                if (children && children.length > 0) {
                    for (var i = 0; i < children.length; i++) {
                        if ($(children[i].target).is(":hidden")) {
                            $(children[i].target).show();
                        }
                    }
                }
            }
        },

        /**  
         * 将滚动条滚动到指定的节点位置，使该节点可见（如果有滚动条才滚动，没有滚动条就不滚动）  
         * @param param {  
         *    treeContainer: easyui tree的容器（即存在滚动条的树容器）。如果为null，则取easyui tree的根UL节点的父节点。  
         *    targetNode:  将要滚动到的easyui tree节点。如果targetNode为空，则默认滚动到当前已选中的节点，如果没有选中的节点，则不滚动  
         * }   
         */
        scrollTo: function (jqTree, param) {
            //easyui tree的tree对象。可以通过tree.methodName(jqTree)方式调用easyui tree的方法  
            var tree = this;

            //如果node为空，则获取当前选中的node  
            var targetNode = param && param.targetNode ? param.targetNode : tree.getSelected(jqTree);

            if (targetNode != null) {
                //判断节点是否在可视区域                 
                var root = tree.getRoot(jqTree);
                var $targetNode = $(targetNode.target);
                var container = param && param.treeContainer ? param.treeContainer : jqTree.parent();
                var containerH = container.height();
                var nodeOffsetHeight = $targetNode.offset().top - container.offset().top;
                if (nodeOffsetHeight > (containerH - 30)) {
                    var scrollHeight = container.scrollTop() + nodeOffsetHeight - containerH + 30;
                    container.scrollTop(scrollHeight);
                }
            }
        }
    });

    /**  
     * 展示搜索匹配的节点  
     */
    function showMatchedNode(jqTree, tree, node) {
        //展示所有父节点  
        $(node.target).show();
        $(".tree-title", node.target).addClass("tree-node-targeted");
        var pNode = node;
        while ((pNode = tree.getParent(jqTree, pNode.target))) {
            $(pNode.target).show();
        }
        //展开到该节点  
        tree.expandTo(jqTree, node.target);
        //如果是非叶子节点，需折叠该节点的所有子节点  
        if (!tree.isLeaf(jqTree, node.target)) {
            //tree.collapse(jqTree, node.target);
        }
    }

    /**  
     * 判断searchText是否与targetText匹配  
     * @param searchText 检索的文本  
     * @param targetText 目标文本  
     * @return true-检索的文本与目标文本匹配；否则为false.  
     */
    function isMatch(searchText, targetText) {
        return $.trim(targetText) != "" && targetText.indexOf(searchText) != -1;
    }

    /**  
     * 获取easyui tree的所有node节点  
     */
    function getAllNodes(jqTree, tree) {
        var allNodeList = jqTree.data("allNodeList");
        if (!allNodeList) {
            var roots = tree.getRoots(jqTree);
            allNodeList = getChildNodeList(jqTree, tree, roots);
            jqTree.data("allNodeList", allNodeList);
        }
        return allNodeList;
    }

    /**  
     * 定义获取easyui tree的子节点的递归算法  
     */
    function getChildNodeList(jqTree, tree, nodes) {
        var childNodeList = [];
        if (nodes && nodes.length > 0) {
            var node = null;
            for (var i = 0; i < nodes.length; i++) {
                node = nodes[i];
                childNodeList.push(node);
                if (!tree.isLeaf(jqTree, node.target)) {
                    var children = tree.getChildren(jqTree, node.target);
                    childNodeList = childNodeList.concat(getChildNodeList(jqTree, tree, children));
                }
            }
        }
        return childNodeList;
    }

    //重新计算datagrid宽度高度（是否显示滚动条）
    $.fn.extend({
        resizeDataGrid: function (heightMargin, widthMargin, minHeight, minWidth) {
            var height = $(document.body).height() - heightMargin;
            var width = $(document.body).width() - widthMargin;
            height = height < minHeight ? minHeight : height;
            width = width < minWidth ? minWidth : width;
            $(this).datagrid('resize', {
                height: height,
                width: width
            });
        }
    });

    //form:表单form id url：提交地址;btn：提交按钮；load，刷新的表格id，num:按钮组，power：权限，errorGrid:错误信息表格id，paras:表单外围参数，layoutId，框架id
    Easy.bindSaveUploadFileForm = function (forms, url, btn, loadId, num, power, errorGrid, paras, layoutId) {
        if ($(btn).linkbutton("options").disabled === false) {
            var buttonCode = power;
            if (Easy.bindPowerValidationEvent(menuId, num, buttonCode)) {
                $(forms).form("submit", {
                    url: url,
                    queryParams: { Paras: JSON.stringify(paras) },
                    onSubmit: function () {
                        var validate = $(forms).form("validate");//验证
                        if (validate) {
                            $(btn).linkbutton("disable");//禁用按钮
                            $.messager.progress({ title: "提示", text: "保存中,请稍后..." });
                        }
                        return validate;
                    },
                    success: function (DATA) {
                        var result = JSON.parse(DATA);
                       
                        $(layoutId).layout('panel', 'center').panel('setTitle', '错误报告');
                        if (result.IsError === false) {
                           
                            $(loadId).datagrid('reload');
                            $(errorGrid).datagrid({ data: [] });
                            $(errorGrid).datagrid("showColumn", "系统备注");
                            if (result.Data!="") {
                                $(errorGrid).datagrid({ data: JSON.parse(result.Data) });
                            }
                            $(forms).form("reset");
                            if (layoutId != "") {
                                $(layoutId).layout("collapse", "north");

                            }
                            Easy.centerShow("系统消息", result.Message, 2000);

                        }
                        else {
                            Easy.centerShow("系统消息", result.Message, 2000);

                        }
                        $(btn).linkbutton("enable");
                        $.messager.progress('close');
                    }
                });
            }
        }
    };



    //上传文件，预览modelurl/模板验证
    Easy.UpLoadFilePreview = function (fileId, uploadfileQueue, gridId, filePath, modelurl, layoutId, title) {
        $(fileId).uploadify({
            'debug': false,
            'preventCaching': false,
            'auto': true,
            'method': 'post',
            'buttonImage': '../Content/Images/uploadpic.png',
            'formData': {
                'folder': '../ELoad/File'
            },
            'swf': "../Content/uploadify/uploadify.swf",
            'queueID': uploadfileQueue,
            'uploader': '../Upload/UploadList',
            'multi': false,
            'width': '80',
            'height': '20',
            'overrideEvents': ['onDialogClose'],
            'fileTypeDesc': '请选择.xls',
            'fileTypeExts': '*.xls',
            'fileSizeLimit': '4MB',
            'removeTimeout': 1,
            'onSelectError': function (file, errorCode, errorMsg) {
                $(layoutId).layout('panel', 'center').panel('setTitle', title);
                $(gridId).datagrid("hideColumn", "系统备注");
                switch (errorCode) {
                    case -100:
                        Easy.centerShow("系统消息", "文件 [" + file.name + "] 上传的文件数量已经超出系统限制！", 2000);
                        break;
                    case -110:
                        Easy.centerShow("系统消息", "文件 [" + file.name + "] 大小超出系统限制的！", 2000);
                        break;
                    case -120:
                        Easy.centerShow("系统消息", "文件 [" + file.name + "] 大小异常！", 2000);
                        break;
                    case -130:
                        Easy.centerShow("系统消息", "文件 [" + file.name + "] 类型不正确！", 2000);
                        break;
                }
            }, 'onFallback': function () {
                Easy.centerShow("系统消息", "您未安装FLASH控件，无法上传图片！请安装FLASH控件后再试。", 2000);
            },
            'onUploadSuccess': function (file, data, response) {
                $(layoutId).layout('panel', 'center').panel('setTitle', title);
                $(gridId).datagrid("hideColumn", "系统备注");
                $.messager.progress({ title: "提示", text: "上传中,请稍后..." });
                setTimeout(function () {
                    var result = Easy.bindSelectInfo(modelurl, data);
                    if (result.IsError === false) {
                        $(filePath).textbox("setValue", data);
                        var tempData = result.Data.replace(/\r/g, '').replace(/\n/g, '');
                        $(gridId).datagrid({ data: JSON.parse(tempData) });
                    }
                    else {
                        $(filePath).textbox("setValue", "");
                        Easy.centerShow("系统消息", result.Message, 2000);
                    }
                    $.messager.progress('close');
                }, 1);
            }
        });
    };
    //折叠面板 处理标题显示
    //$.extend($.fn.layout.paneldefaults, {
    //    onCollapse: function () {
    //        //获取layout容器 
    //        var layout = $(this).parents("div.layout");
    //        //获取当前region的配置属性 
    //        var opts = $(this).panel("options");
    //        //获取key 
    //        var expandKey = "expand" + opts.region.substring(0, 1).toUpperCase() + opts.region.substring(1);
    //        //从layout的缓存对象中取得对应的收缩对象 
    //        var expandPanel = layout.data("layout").panels[expandKey];
    //        //针对横向和竖向的不同处理方式 
    //        if (opts.region == "west" || opts.region == "east") {
    //            //竖向的文字打竖,其实就是切割文字加br 
    //            var split = [];
    //            for (var i = 0; i < opts.title.length; i++) {
    //                split.push(opts.title.substring(i, i + 1));
    //            }
    //            expandPanel.panel("body").addClass("panel-title").css("text-align", "center").html(split.join("<br>"));
    //        }
    //        else {
    //            expandPanel.panel("setTitle", opts.title);
    //        }

    //    }
    //});
    $.extend($.fn.validatebox.defaults.rules, {
        idCard: {
            validator: function (value, param) {
                var result = Easy.bindSelectInfo("../Common/CheckIdCard", value);
                if (result.IsError === false) {
                    return true;
                }
                else {
                    return false;
                }
            },
            message: '必须是合法的中国身份证'
        }
    });
    //关闭窗口的同时销毁uploadfiy
    Easy.closeWindow = function (id, uploadfiy) {
        $('#' + id + '').dialog('close');
        $('#' + uploadfiy + '').uploadify("destroy");
    }
    Easy.bindSelectStudentInfo = function (studentId) {
        var result = Easy.bindSelectInfo("../Student/SelectViewStudent", studentId);
        $("#spStudentName").html(result.Data[0].Name);
        $("#spStudentIDCard").html(result.Data[0].IDCard);
        $("#spSex").html(result.Data[0].Sex);
        $("#spMobile").html(result.Data[0].Mobile);
        $("#spQQ").html(result.Data[0].QQ);
        $("#spWeChat").html(result.Data[0].WeChat);
        $("#spAddress").html(result.Data[0].Address);
        $("#spStudentRemark").html(result.Data[0].Remark);
        var result1 = Easy.bindSelectInfo("../Student/SelectViewStudentInfo", studentId);
        var info = JSON.parse(result1.Data);
        if (info.length === 1) {
            $("#spPC").html(info[0].PC);
            $("#spNation").html(info[0].Nation);
            $("#spZip").html(info[0].Zip);
            $("#spSchool").html(info[0].School);
        }


        $("#studentContanctgrid").datagrid({
            striped: true,//斑马线
            rownumbers: true,//行号
            singleSelect: true,//只允许选择一行
            pagination: true,//分页
            pageSize: 20,
            queryParams: { StudentID: studentId },//异步查询的参数
            columns: [[
                { field: "Name", title: "姓名", width: 150, sortable: true },
                { field: "Tel", title: "联系电话", width: 150, sortable: true }
            ]],
            url: "../StudentContact/GetStudentContactList"
        });
    }


    Easy.PrintsFeeDetatilNew = function (model, noteNum) {
        var printString = "";;
        printString += '<table style="width: 460px;">';
        printString += '<tr>';
        printString += '<td style="font-size: 18px; font-weight: bold; text-align: center; line-height:30px;">' + model.DeptName + '学费收据</td>';
        printString += '</tr>';
        printString += '</table>';
        printString += '<table style="width: 460px; font-size: 14px; line-height:30px;">';
        printString += '<tr>';
        printString += '<td style="width: 60px; text-align: right;">凭证号:</td>';
        printString += '<td style="width: 200px;">' + model.VoucherNum + '</td>';
        printString += '<td style="width: 20px;"></td>';
        printString += '<td style="width: 100px; text-align: right;">票据号:</td>';
        printString += '<td style="width: 80px;">' + noteNum + '</td>';
        printString += '</tr>';
        printString += '</table>';
        printString += '<table cellpadding="0" cellspacing="0" style="width: 460px; border-collapse: collapse; font-size: 14px; text-align: center; line-height: 25px;">';
        printString += '<tr>';
        printString += '<td style="border: 1px solid #000000;">收费主体:</td>';
        printString += '<td style="border: 1px solid #000000; text-align:left; padding-left:5px;" colspan="5">' + model.SchoolName + '</td>';
        printString += '</tr>';
        printString += '<tr>';
        printString += '<td style="border: 1px solid #000000;">就读校区:</td>';
        printString += '<td style="border: 1px solid #000000; text-align:left; padding-left:5px;">' + model.DeptAreaName + '</td>';
        printString += '<td style="border: 1px solid #000000;">就读专业:</td>';
        printString += '<td style="border: 1px solid #000000; text-align:left; padding-left:5px;" colspan="3">' + model.Major + '</td>';
        printString += '</tr>';
        printString += '<tr>';
        printString += '<td style="border: 1px solid #000000;">学号:</td>';
        printString += '<td style="border: 1px solid #000000; text-align: left; padding-left: 5px;">' + model.EnrollNum + '</td>';
        printString += '<td style="border: 1px solid #000000;">缴费期数:</td>';
        printString += '<td style="border: 1px solid #000000; text-align :left; padding-left: 5px;" colspan="3">' + model.NumName + '</td>';
        printString += '</tr>';
        printString += '<tr>';
        printString += '<td style="border: 1px solid #000000;">缴费人:</td>';
        printString += '<td style="border: 1px solid #000000; text-align: left; padding-left: 5px;">' + model.StuName + '</td>';
        printString += '<td style="border: 1px solid #000000;">证件号码:</td>';
        printString += '<td style="border: 1px solid #000000; text-align: left; padding-left: 5px;" colspan="3">' + model.IDCard + '</td>';
        printString += '</tr>';
        printString += '<tr>';
        printString += '<td style="border: 1px solid #000000;" colspan="6">';
        printString += '<table style="font-size:12px; text-align: center;">';
        printString += '<tr>';
        printString += '<td style="width:130px;">费用类别</td>';
        printString += '<td style="width:66px;">应收金额</td>';
        printString += '<td style="width:66px;">已收金额</td>';
        printString += '<td style="width:66px;">实收金额</td>';
        printString += '<td style="width:66px;">充抵金额</td>';
        printString += '<td style="width:66px;">优惠金额</td>';
        printString += '</tr>';
        printString += '</table>';
        printString += '</td>';
        printString += '</tr>';
        printString += '<tr>';
        printString += '<td style="border: 1px solid #000000;" colspan="6">';
        printString += '<table style="font-size:12px; text-align: center;">';
        var temp = "";
        model.sfeeContent = '<Detail>' + model.sfeeContent + ' </Detail>'
        var xml = $.parseXML(model.sfeeContent);

        $(xml).find('sFeeDetail').each(function () {
            var detail = $(this).text();
            var temp = detail.split(',');
            printString += '<tr>';
            printString += '<td style="width:130px;">' + temp[0] + '</td>';
            printString += '<td style="width:66px;">' + temp[1] + '</td>';
            printString += '<td style="width:66px;">' + temp[2] + '</td>';
            printString += '<td style="width:66px;">' + temp[3] + '</td>';
            printString += '<td style="width:66px;">' + temp[4] + '</td>';
            printString += '<td style="width:66px;">' + temp[5] + '</td>';
            printString += '</tr>';
        });
        printString += '</table>';
        printString += '</td>';
        printString += '</tr>';
        printString += '<tr>';
        printString += '<td style="border: 1px solid #000000;" colspan="6">';
        printString += '<table style="font-size:12px; text-align: center;">';
        printString += '<tr>';
        printString += '<td style="width:130px;">合计</td>';

        var shouldMoney = 0;
        var paidMoney = 0;
        var money = 0;
        var discountMoney = 0;
        var offsetMoney = 0;
        $(xml).find('sFeeDetail').each(function () {
            var detail = $(this).text();
            var temp = detail.split(',');
            shouldMoney = parseFloat(shouldMoney) + parseFloat(temp[1]);
            paidMoney = parseFloat(paidMoney) + parseFloat(temp[2]);
            money = parseFloat(money) + parseFloat(temp[3]);
            discountMoney = parseFloat(discountMoney) + parseFloat(temp[4]);
            offsetMoney = parseFloat(offsetMoney) + parseFloat(temp[5]);

        });
        printString += '<td style="width:66px;">' + shouldMoney.toFixed("2") + '</td>';
        printString += '<td style="width:66px;">' + paidMoney.toFixed("2") + '</td>';
        printString += '<td style="width:66px;">' + money.toFixed("2") + '</td>';
        printString += '<td style="width:66px;">' + discountMoney.toFixed("2") + '</td>';
        printString += '<td style="width:66px;">' + offsetMoney.toFixed("2") + '</td>';
        printString += '</tr>';
        printString += '</table>';
        printString += '</td>';
        printString += '</tr>';
        if (model.GiveName != null && model.GiveName != "") {
            var give = $.parseXML(model.GiveName);
            var giveString = "";
            $(give).find('Name').each(function () {
                giveString += detail = $(this).text() + ',';
            });
            giveString = giveString.substring(0, giveString.length - 1);
            printString += '<tr>';
            printString += '<td style="border: 1px solid #000000;">配品:</td>';
            printString += '<td style="border: 1px solid #000000; text-align: left; padding-left: 5px; width: 310px;" colspan="5">' + giveString + '</td>';
            printString += '</tr>';
        }

        printString += '<tr>';
        printString += '<td style="border: 1px solid #000000; width:70px;">收费员:</td>';
        printString += '<td style="border: 1px solid #000000; width:80px; text-align :left; padding-left: 5px;">' + model.UserName + '</td>';
        printString += '<td style="border: 1px solid #000000; width:70px;">缴费方式:</td>';
        printString += '<td style="border: 1px solid #000000; width:80px; text-align: left; padding-left: 5px;">' + model.FeeMode + '</td>';
        printString += '<td style="border: 1px solid #000000; width:70px;">收费时间:</td>';
        printString += '<td style="border: 1px solid #000000; width:90px; text-align: left; padding-left: 5px;">' + model.Explain + '</td>';
        printString += '</tr>';
        printString += '<tr>';
        printString += '<td style="border: 1px solid #000000;">备注:</td>';
        printString += '<td style="border: 1px solid #000000; text-align: left; padding-left: 5px; width: 310px;" colspan="5">' + model.FeeTime + '</td>';
        printString += '</tr>';
        printString += '</table>';
        return printString;
    }

    Easy.PrintsFeeTotalNew = function (model, noteNum) {
        var printString = "";
        printString += '<table style="width: 460px;">';
        printString += '<tr>';
        printString += '<td style="font-size: 18px; font-weight: bold; text-align: center; line-height:30px;">' + model.DeptName + '学费收据</td>';
        printString += '</tr>';
        printString += '</table>';
        printString += '<table style="width: 460px; font-size: 14px; line-height:30px;">';
        printString += '<tr>';
        printString += '<td style="width: 60px; text-align: right;">凭证号:</td>';
        printString += '<td style="width: 200px;">' + model.VoucherNum + '</td>';
        printString += '<td style="width: 20px;"></td>';
        printString += '<td style="width: 100px; text-align: right;">票据号:</td>';
        printString += '<td style="width: 80px;">' + noteNum + '</td>';
        printString += '</tr>';
        printString += '</table>';
        printString += '<table cellpadding="0" cellspacing="0" style="width: 460px; border-collapse: collapse; font-size: 14px; text-align: center; line-height: 35px;">';
        printString += '<tr>';
        printString += '<td style="border: 1px solid #000000;">收费主体:</td>';
        printString += '<td style="border: 1px solid #000000; text-align:left; padding-left:5px;" colspan="3">' + model.SchoolName + '</td>';
        printString += '<td style="border: 1px solid #000000;">就读校区:</td>';
        printString += '<td style="border: 1px solid #000000; text-align:left; padding-left:5px;">' + model.DeptAreaName + '</td>';
        printString += '</tr>';
        printString += '<tr>';
        printString += '<td style="border: 1px solid #000000;">学号:</td>';
        printString += '<td style="border: 1px solid #000000; text-align: left; padding-left: 5px;">' + model.EnrollNum + '</td>';
        printString += '<td style="border: 1px solid #000000;">就读专业:</td>';
        printString += '<td style="border: 1px solid #000000; text-align:left; padding-left:5px;" colspan="3">' + model.Major + '</td>';
        printString += '</tr>';
        printString += '<tr>';
        printString += '<td style="border: 1px solid #000000;">缴费人:</td>';
        printString += '<td style="border: 1px solid #000000; text-align: left; padding-left: 5px;">' + model.StuName + '</td>';
        printString += '<td style="border: 1px solid #000000;">证件号码:</td>';
        printString += '<td style="border: 1px solid #000000; text-align: left; padding-left: 5px;" colspan="3">' + model.IDCard + '</td>';
        printString += '</tr>';
        printString += '<tr>';
        printString += '<td style="border: 1px solid #000000;" colspan="6">';
        printString += '<table style="font-size:14px; text-align: center; line-height:30px;">';
        printString += '<tr>';
        printString += '<td style="width:70px;"></td>';
        printString += '<td style="width:78px;">应收金额</td>';
        printString += '<td style="width:78px;">已收金额</td>';
        printString += '<td style="width:78px;">实收金额</td>';
        printString += '<td style="width:78px;">充抵金额</td>';
        printString += '<td style="width:78px;">优惠金额</td>';
        printString += '</tr>';
        printString += '</table>';
        printString += '</td>';
        printString += '</tr>';
        printString += '<tr>';
        printString += '<td style="border: 1px solid #000000;" colspan="6">';
        var temp = "";
        model.sfeeContent = '<Detail>' + model.sfeeContent + ' </Detail>'
        var xml = $.parseXML(model.sfeeContent);
        var shouldMoney = 0;
        var paidMoney = 0;
        var money = 0;
        var discountMoney = 0;
        var offsetMoney = 0;
        $(xml).find('sFeeDetail').each(function () {
            var detail = $(this).text();
            var temp = detail.split(',');
            shouldMoney = parseFloat(shouldMoney) + parseFloat(temp[1]);
            paidMoney = parseFloat(paidMoney) + parseFloat(temp[2]);
            money = parseFloat(money) + parseFloat(temp[3]);
            discountMoney = parseFloat(discountMoney) + parseFloat(temp[4]);
            offsetMoney = parseFloat(offsetMoney) + parseFloat(temp[5]);

        });
        printString += '<table style="font-size:14px; text-align: center; line-height:30px;">';
        printString += '<tr>';
        printString += '<td style="width:70px;">合计</td>';
        printString += '<td style="width:78px;">' + shouldMoney + '</td>';
        printString += '<td style="width:78px;">' + paidMoney + '</td>';
        printString += '<td style="width:78px;">' + money + '</td>';
        printString += '<td style="width:78px;">' + discountMoney + '</td>';
        printString += '<td style="width:78px;">' + offsetMoney + '</td>';
        printString += '</tr>';
        printString += '</table>';
        printString += '</td>';
        printString += '</tr>';
        if (model.GiveName != null && model.GiveName != "") {
            var give = $.parseXML(model.GiveName);
            var giveString = "";
            $(give).find('Name').each(function () {
                giveString += detail = $(this).text() + ',';
            });
            giveString = giveString.substring(0, giveString.length - 1);
            printString += '<tr>';
            printString += '<td style="border: 1px solid #000000;">配品:</td>';
            printString += '<td style="border: 1px solid #000000; text-align: left; padding-left: 5px; width: 310px;" colspan="5">' + giveString + '</td>';
            printString += '</tr>';
        }
        printString += '<tr>';
        printString += '<td style="border: 1px solid #000000; width:70px;">收费员:</td>';
        printString += '<td style="border: 1px solid #000000; width:80px; text-align :left; padding-left: 5px;">' + model.UserName + '</td>';
        printString += '<td style="border: 1px solid #000000; width:70px;">缴费方式:</td>';
        printString += '<td style="border: 1px solid #000000; width:80px; text-align: left; padding-left: 5px;">' + model.FeeMode + '</td>';
        printString += '<td style="border: 1px solid #000000; width:70px;">收费时间:</td>';
        printString += '<td style="border: 1px solid #000000; width:90px; text-align: left; padding-left: 5px;">' + model.FeeTime + '</td>';
        printString += '</tr>';
        printString += '<tr>';
        printString += '<td style="border: 1px solid #000000;">备注:</td>';
        printString += '<td style="border: 1px solid #000000; text-align: left; padding-left: 5px; width: 310px;" colspan="5">' + model.Explain + '</td>';
        printString += '</tr>';
        printString += '</table>';
        return printString;
    }
})(jQuery);
var sEnrollsProfessionID = "0 ";

var sEnrollsProfessionIDs = "";

var sEnrollMajor = "0";

var sEnrollYear = "0";

var sEnrollMonth = "0";

var sEnrollSchemeTempData = "";
var stuId;
var name;
var idCard;
//var sEnrollTime;
var rowData;
var chooseSchame = "0";
var enrollLevel;
firstFunction = function () {
    //加载表格


    bindToolBarMethod();
    initTable({ MenuID: menuId });

    //加载搜索表单数据
    bindSearchFormEvent();

    //加载搜索按钮的点击事件
    bindSearchClickEvent();

    //加载开关高级搜索事件
    Easy.bindOpenCloseSearchBoxEvent(120);

    //添加、修改保存按钮
    Easy.bindSaveButtonClickEvent("#editsEnroll", "#fSave", "../sEnroll/GetsEnrollEdit", "#btnSave", "1", "#grid", menuId, "1", "#editsEnrollsProfessionID");
    //审查
    Easy.bindSaveButtonClickEvent("#sEnrollAudit", "#fAudit", "../sEnroll/GetsEnrollAudit", "#btnSavesEnrollAudit", "1", "#grid", menuId, "1", "#audId");
    //转正报
    Easy.bindSaveButtonClickEvent("#sEnrollTurn", "#fturnSave", "../sEnroll/GetsEnrollTurn", "#btnSaveTurn", "1", "#grid", menuId, "1", "#Id");

    //添加
    tempBindSaveButtonClickEvent("#addsEnroll", "#faddSave", "../sEnroll/AddEnroll", "#btnSaveAdd", "1", "#grid", menuId, "1", "");
    //添加并生成
    bindAddCoButtonClickEvent(menuId, "1");
    //报名信息停用保存按钮
    Easy.bindSaveButtonClickEventBybtnCode("#sEnrollStop", "#fSaveStop", "../sEnroll/GetsEnrollStop", "#btnSavesEnrollStop", "1", "#grid", menuId, "1", "disable");

    //转专业保存按钮
    tempBindSaveButtonClickEventBybtnCode("#sEnrollTurnMajor", "#fSaveMajor", "../sEnroll/GetsEnrollMajor", "#btnSavesEnrollTurnMajor", "1", "#grid", menuId, "1", "trunMajor");
    //转专业保存并生成按钮
    bindTurnMajorCoButtonClickEvent(menuId, "1");

    Easy.bindSaveButtonClickEvent("#editsEnroll", "#fSave", "../sEnroll/GetsEnrollEdit", "#btnSave", "1", "#grid", menuId, "1", "#editsEnrollsProfessionID");
    //上传保存按钮
    // Easy.bindSaveUploadFile("#upsEnroll", "#fUpload", "../sEnroll/UpLoadsEnroll", "#btnSaveUpsEnroll", "1", "#grid", menuId, "1", "upload");

    bindBulildMoreButton();
    bindSaveUplodMethod();
    bindSaveModStatusMethod();
    bindSaveUplodModifyStatusMethod();
    bindSaveClassgroupMethod();
    bindSavaModifyClassMetod();
    bindSavaModifyLeaveYearMethod();
    bindSaveUplodModifyLeaveYearMethod();
    bindSavaTurnOfficial();
    bindSaveEnrollRefund();
    bindSaveUplodEnrollDeptMethod();
    bindSaveUploadStudyNum();
}

//加载表格
initTable = function (queryData) {
    $("#grid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: false,//只允许选择一行
        pagination: true,//分页
        showFooter: true,
        pageSize: 20,
        nowrap: true,
        queryParams: queryData,//异步查询的参数ChangeStatusReson
        columns: [[
            { field: "sEnrollsProfessionID", checkbox: true },
            { field: "StudentID", hidden: true },
            {
                field: "StatusName", title: "状态", width: 60, sortable: true,
                formatter: function (value, row, index) {
                    var str = "";
                    if (row.ChangeStatusReson != "" && row.ChangeStatusReson != null && row.ChangeStatusReson != undefined) {
                        var xml = $.parseXML(row.ChangeStatusReson);
                        $(xml).find('status').each(function () {
                            var name = $(this).children('RefeName').text();
                            var createTime = $(this).children('CreateTime').text();
                            var explain = $(this).children('Explain').text();
                            if (row.StatusName != name) {
                                str = value;
                            } else {
                                str = "<span title=\"变更时间:" + createTime + "<br /> 变更原因:" + explain + "\" class=\"tip\" style=\"text-decoration: underline;cursor: pointer;\">" + value + "</span>";
                            }
                        });
                    }
                    else {
                        str = value;
                    }
                    return str;
                }
            },
            { field: "DeptName", title: "报名学校", width: 200, sortable: true },
            { field: "DeptAreaName", title: "报名校区", width: 60, sortable: true },
            { field: "ExamNum", title: "考生号", width: 140, sortable: true },
            { field: "StudentName", title: "学生姓名", width: 60, sortable: true },
            { field: "EnrollNum", title: "学号", width: 100, sortable: true },
            { field: "IDCard", title: "身份证号", width: 140, sortable: true },
            { field: "Year", title: "年份", width: 80, sortable: true },
            { field: "Month", title: "月份", width: 80, sortable: true },
            { field: "PlanSort", title: "报读类别", width: 80, sortable: true },
            { field: "Level", title: "学习层次", width: 80, sortable: true },
            { field: "Major", title: "专业", width: 140, sortable: true },
            { field: "ClassName", title: "班级", width: 140, sortable: true },
            {
                field: "BeforeEnrollTime", title: "预报名时间", width: 80, sortable: true, formatter: function (value, row, index) {
                    if (value === "1900-01-01")
                        return "";
                    return value;
                }
            },
            {
                field: "SignTime", title: "正式报名时间", width: 80, sortable: true, formatter: function (value, row, index) {
                    if (value === "1900-01-01")
                        return "";
                    return value;
                }
            }, {
                field: "FirstFeeTime", title: "第一次缴费时间", width: 80, sortable: true, formatter: function (value, row, index) {
                    if (value === "1900-01-01")
                        return "";
                    return value;
                }
            },
        {
            field: "LeaveYear", title: "离校年度", width: 80, sortable: true
        },
        {
            field: "ShouldMoney", title: "总学费金额", sortable: true, halign: 'left', align: 'right'
        },
        {
            field: "PaidMoney", title: "已供贷金额", sortable: true, halign: 'left', align: 'right'
        },
        {
            field: "NoMoney", title: "未供贷金额", sortable: true, halign: 'left', align: 'right'
        },
        {
            field: "NoMoneyNoGive", title: "逾期欠费金额", sortable: true, halign: 'left', align: 'right'
        },
        {
            field: "NoGive", title: "逾期欠费(补贴)", sortable: true, halign: 'left', align: 'right'
        }, {
            field: "CreateTime", title: "创建时间", width: 80, sortable: true, formatter: function (value, row, index) {
                if (value === "1900-01-01")
                    return "";
                return value;
            }
        }
            //,
            //{
            //    field: "sOrderStatus", title: "缴费单状态", width: 80, sortable: true, formatter: function (value, row, index) {
            //        if (row.sEnrollsProfessionID != "" && row.sEnrollsProfessionID != undefined) {
            //            return GetsOrderStatus(row.sEnrollsProfessionID);
            //        }
            //    }
            //}
        ]],
        url: "../sEnroll/GetsEnrollList", sortName: "sEnrollsProfessionID", sortOrder: "desc",
        rowStyler: function (index, row) {
            if (row.Status === "9") {
                return "color:#ff0000;";
            }
            else if (row.Status === "3") {
                return "color:#66B3FF;";
            }
            else if (row.Status === "4") {
                return "color:#339900;";
            }
            else if (row.Status === "5") {
                return "color:#FF60AF;";
            }
            else if (row.Status === "6") {
                return "color:#1d5ea0;";
            }
            else if (row.Status === "7") {
                return "color:#FF60AF;";
            }
            else if (row.Status === "10") {
                return "color:#FF60AF;";
            }
        },
        onBeforeLoad: function (param) {
            //$('#grid').datagrid('reloadFooter', JSON.parse(Easy.bindSelectInfomation("../sEnroll/GetsEnrollListFoot", queryData).Data));
        },
        onLoadSuccess: function (data) {
            Easy.bindCustomPromptToTableEvent(".tip");
            $('#grid').resizeDataGrid(0, 0, 0, 0);//重新计算宽度高度
        }
    });

}
bindToolBarMethod = function () {
    $("#gridtoolbar").html(Easy.loadToolbarHtml(menuId, "1"));
    $('#grid').datagrid({
        toolbar: '#gridtoolbar'
    });
    $.parser.parse('#gridtoolbar');
}
//绑定搜索按钮的点击事件
bindSearchClickEvent = function () {
    $("#btnSearch").click(function () {//按条件进行查询数据，首先我们得到数据的值
        var queryData = rerurnQueryData();
        initTable(queryData);//将值传递给
    });
    $("#btnReset").click(function () {
        $("#fSearch").form("reset");//重置表单
        initTable({
            MenuID: menuId
        });
    });
}

//绑定搜索表单数据
bindSearchFormEvent = function () {
    $("#selPlanSort").combobox({
        url: "../Refe/SelList?RefeTypeID=14",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 200,
        panelWidth: 120,
        multiple: true,
        editable: false
    });
    $("#treeDept").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: {
            MenuID: menuId, IsEdit: "No", Status: "1"
        },
        animate: true,
        lines: true,
        panelWidth: 300
    });

    $("#selLevel").combobox({
        url: "../Refe/SelList?RefeTypeID=17",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 200,
        panelWidth: 120,
        multiple: true,
        editable: false
    });

    $("#selYear").combobox({
        url: "../Common/YearCombobox?Year=2010&IsSelected=No&Is=No",
        valueField: "Id",
        textField: "Text",
        panelHeight: 120,
        multiple: true,
        editable: false
    });

    $("#txtLeaveYear").combobox({
        url: "../Common/LeaveYearCombobox",
        valueField: "id",
        textField: "text",
        queryParams: { startYear: "2010", isSelected: false },
        panelHeight: 120,
        multiple: true,
        editable: false
    });

    $("#selMonth").combobox({
        url: "../Refe/SelList?RefeTypeID=18",
        valueField: "Value",
        textField: "RefeName",
        multiple: true,
        editable: false
    });

    $("#selStatus").combobox({
        url: "../Refe/SelList?RefeTypeID=21",
        valueField: "Value",
        textField: "RefeName",
        multiple: true,
        editable: false
    });

    $("#selNo").combobox({
        data: [{
            Id: '1',
            Text: '逾期欠费'
        }, {
            Id: '2',
            Text: '逾期欠费(补助)'
        }],
        valueField: "Id",
        textField: "Text",
        editable: false
    });
}

//绑定批量生成按钮事件
bindBulildMoreButton = function () {
    $("#btnSavesEnrollBuildMore").click(function () {
        if (Easy.bindPowerValidationEvent(menuId, "1", "generateMore")) {
            var a = $("#buildsProfessionID").combobox("getValues");
            var b = $("#buildScheme").combobox("getValues");
            if (a == "" && b == "") {
                Easy.centerShow("系统消息", "专业和缴费方案必选选择一个", 3000);
            }
            else {
                var buildQueryData = rerurnBuildQueryData();
                //批量生成
                $.messager.confirm("", "确定要生成缴费单吗？", function (c) {
                    if (c) {
                        $.ajax({
                            type: "post",
                            url: "../sEnroll/GetBuildsProfessionIDs",
                            async: false,
                            data: buildQueryData,
                            dataType: "text",
                            success: function (data) {
                                if (data != null && data != "") {
                                    sEnrollsProfessionIDs = data;
                                    $('#sEnrollBuildMore').dialog('close');
                                    $("#sEnrollBuild").dialog({
                                        title: "生成缴费单",
                                        width: 680,
                                        height: 380,
                                        href: "../sEnroll/sEnrollBuild",
                                        buttons: "#sEnrollBuild-buttons",
                                        modal: true,
                                        closable: false
                                    });
                                }
                                else {
                                    Easy.centerShow("系统消息", "没有可生成的报名数据", 3000);
                                }
                            },
                            error: function () {
                                Easy.centerShow("系统消息", "出现未知错误，请联系管理员", 3000);
                            }
                        });
                    }
                });
            }
        }
    });
}

//导入
upload = function () {
    //var res = Easy.bindSelectInfo("../Area/GetProvinceFile", "");
    //if (res.IsError===false) {
    //    Easy.centerShow("系统消息", "生成成功", 3000);
    //}
    if (Easy.bindPowerValidationEvent(menuId, "1", "upload")) {
        Easy.OpenDialogEvent("#upsEnroll", "导入报名信息", 680, 550, "../sEnroll/sEnrollUpload", "#upsEnroll-buttons");
    }
}

//添加
add = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "add")) {
        sEnrollsProfessionID = "0";
        Easy.OpenDialogEvent("#addsEnroll", "添加报名信息", 680, 610, "../sEnroll/sEnrollAdd", "#addsEnroll-buttons");
    }
}

//修改
edit = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "edit")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "报名信息")) {
            //验证报名专业是否已经缴费
            sEnrollsProfessionID = rows[0].sEnrollsProfessionID;
            Easy.OpenDialogEvent("#editsEnroll", "编辑报名信息", 680, 380, "../sEnroll/sEnrollEdit", "#editsEnroll-buttons");

        }
    }
}

//审查
audit = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "audit")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "报名信息")) {
            if (rows[0].Status != "1") {
                Easy.centerShow("系统消息", "该报名信息不符合审核条件!", 2000);
                return false;
            }
            rowData = rows[0];
            Easy.OpenDialogEvent("#sEnrollAudit", "审查报名信息", 400, 300, "../sEnroll/sEnrollAudit", "#sEnrollAudit-buttons");
        }
    }
}

//转专业
trunMajor = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "trunMajor")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        //验证是否选中行
        if (Easy.checkRow(rows, "报名信息")) {
            var result = Easy.bindSelectInfo("../sEnroll/GetsEnrollsProfessionStatus", rows[0].sEnrollsProfessionID);
            if (result.Message != "9") {
                Easy.OpenDialogEvent("#sEnrollTurnMajor", "转专业", 700, 450, "../sEnroll/TurnMajor", "#sEnrollTurnMajor-buttons");
            }
            else {
                Easy.centerShow("系统消息", "该报名信息已停用，不能转专业", 3000);
            }
        }
    }
}

//反审查
noaudit = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "noaudit")) {
        bindUpdateStatus("反审查", "1");
    }
}

//停用
disable = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "disable")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        //验证是否选中行
        if (Easy.checkRow(rows, "报名信息")) {
            var result = Easy.bindSelectInfo("../sEnroll/GetsEnrollsProfessionStatus", rows[0].sEnrollsProfessionID);
            if (result.Message != "9") {
                Easy.OpenDialogEvent("#sEnrollStop", "停用报名信息", 600, 240, "../sEnroll/StopsEnroll", "#sEnrollStop-buttons");
            }
            else {
                Easy.centerShow("系统消息", "该报名信息已停用", 3000);
            }
        }
    }
}

//查看
view = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "view")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "报名信息")) {
            sEnrollsProfessionID = rows[0].sEnrollsProfessionID;
            Easy.OpenDialogEvent("#sEnrollDetail", "查看信息", 800, 600, "../sEnroll/sEnrollDetail", "#sEnrollDetail-buttons");
        }
    }
}

//生成交费单
generate = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "generate")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRows(rows, "报名信息")) {
            var rowsIdStr = "";
            for (var i = 0; i < rows.length; i++) {
                rowsIdStr += rows[i].sEnrollsProfessionID + ",";
            }
            if (rowsIdStr != "") {
                rowsIdStr = rowsIdStr.substring(0, rowsIdStr.length - 1);
            }
            //验证选择的报名信息中是否有不能生成的数据（只有在报名状态下才能生成缴费单）
            var result = Easy.bindSelectInfo("../sEnroll/CheckIsBuildsFee", rowsIdStr);
            if (result.Data) {
                Easy.centerShow("系统消息", "在录取或报名状态下才能生成缴费单", 3000);
            }
            else {
                //生成
                $.messager.confirm("", "确定要生成缴费单吗？", function (c) {
                    if (c) {
                        sEnrollsProfessionIDs = rowsIdStr;
                        $("#sEnrollBuild").dialog({
                            title: "生成缴费单",
                            width: 680,
                            height: 380,
                            href: "../sEnroll/sEnrollBuild",
                            buttons: "#sEnrollBuild-buttons",
                            modal: true,
                            closable: false
                        });
                    }
                });
            }
        }
    }
}

//批量生成缴费单
generateMore = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "generateMore")) {
        Easy.OpenDialogEvent("#sEnrollBuildMore", "批量生成缴费单", 560, 220, "../sEnroll/sEnrollBuildMore", "#sEnrollBuildMore-buttons");
    }
}

//导出
derive = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "derive")) {
        var queryData = rerurnQueryData();
        $.ajax({
            type: "post",
            url: "../sEnroll/DownloadsEnroll",
            async: false,
            data: queryData,
            dataType: "json",
            success: function (result) {
                if (result.IsError === false) {
                    location.href = "../Temp/" + result.Message;
                }
            }
        });
    }
}

//导出报名模板
deriveTemplate = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "deriveTemplate")) {
        location.href = "../ModelExcel/录取信息导入模板.xls";
    }
}

bindUpdateStatus = function (confirmstr, status) {
    var rows = $("#grid").datagrid("getSelections");//选中的所有ID
    //验证是否选中行
    if (Easy.checkRow(rows, "报名信息")) {
        //修改状态
        Easy.bindUpdateValue(confirmstr, status, "../sEnroll/GetUpdatesStatus", rows[0].sEnrollsProfessionID, "1", "#grid");
    }
}

rerurnQueryData = function () {
    var queryData = {//得到用户输入的参数
        MenuID: menuId,
        txtStuName: $("#txtStuName").textbox("getValue"),
        txtExamNum: $("#txtExamNum").textbox("getValue"),
        txtEnrollNum: $("#txtEnrollNum").textbox("getValue"),
        txtIDCard: $("#txtIDCard").textbox("getValue"),
        txtSignTimeS: $("#txtSignTimeS").datebox("getValue"),
        txtSignTimeE: $("#txtSignTimeE").datebox("getValue"),

        txtBeforeEnrollTimeS: $("#txtBeforeEnrollTimeS").datebox("getValue"),
        txtBeforeEnrollTimeE: $("#txtBeforeEnrollTimeE").datebox("getValue"),
        txtFirstFeeTimeE: $("#txtFirstFeeTimeE").datebox("getValue"),
        txtFirstFeeTimeS: $("#txtFirstFeeTimeS").datebox("getValue"),
        txtCreateTimeS: $("#txtCreateTimeS").datebox("getValue"),
        txtCreateTimeE: $("#txtCreateTimeE").datebox("getValue"),

        treeDept: $("#treeDept").combotree("getValue"),
        selLevel: "" + $("#selLevel").combobox("getValues"),
        txtMajor: $("#txtMajor").textbox("getValue"),
        txtClass: $("#txtClass").textbox("getValue"),
        selYear: "" + $("#selYear").combobox("getValues"),
        selMonth: "" + $("#selMonth").combobox("getValues"),
        selStatus: "" + $("#selStatus").combobox("getValues"),
        selNo: "" + $("#selNo").combobox("getValue"),
        txtLeaveYear: "" + $("#txtLeaveYear").combobox("getValues"),
        selPlanSort: "" + $("#selPlanSort").combobox("getValues")

    }
    return queryData;
}

rerurnBuildQueryData = function () {
    var queryData = {
        buildEnrollLevel: $("#buildEnrollLevel").combobox("getValue"),
        buildsProfessionID: "" + $("#buildsProfessionID").combobox("getValues"),
        buildScheme: "" + $("#buildScheme").combobox("getValues")
    }
    return queryData;
}

GetsOrderStatus = function (sEnrollsProfessionID) {
    var result1 = Easy.bindSelectInfo("../sEnroll/GetsOrderStatus", sEnrollsProfessionID);
    return result1.Message;
}

turnOfficial = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "turnOfficial")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "报名信息")) {
            sEnrollsProfessionID = rows[0].sEnrollsProfessionID;
            stuId = rows[0].StudentID;
            name = rows[0].StudentName;
            idCard = rows[0].IDCard;
            //sEnrollTime = rows[0].SignTime;
            if (rows[0].Status != "2") {

                Easy.centerShow("系统消息", "只有预报名学生才能转正式报名", 2000);
                return false;
            }
            Easy.OpenDialogEvent("#sEnrollTurn", "转正式报名", 400, 200, "../sEnroll/sEnrollTurnOfficial", "#sEnrollTurn-buttons");
        }
    }
}

//点击保存按钮（弹出窗口id,表单id,提交地址,提交按钮id,刷新类别(1:表格2:树3:方法4:treegrid),刷新id(如类别是3则是方法名),菜单id,按钮分组,主键id）
tempBindSaveButtonClickEvent = function (dialog, form, url, btn, type, loadId, menuId, num, id) {
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
                            $("#btnSaveAddCo").linkbutton("disable");//禁用按钮
                            $(btn).linkbutton("disable");//禁用按钮
                        }
                        return validate;
                    },
                    success: function (data) {
                        var result = JSON.parse(data);
                        if (result.IsError === false) {
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
                            Easy.centerShow("系统消息", result.Message, 3000);
                        }
                        $("#btnSaveAddCo").linkbutton("enable");//解除按钮禁用
                        $(btn).linkbutton("enable");//解除按钮禁用
                    }
                });
            }
        }
    });
}

bindAddCoButtonClickEvent = function (menuId, num) {
    $("#btnSaveAddCo").click(function () {
        if ($("#btnSaveAddCo").linkbutton("options").disabled === false) {//按钮没有禁用才能执行（easyui特殊处理）
            var buttonCode = "add";
            if (Easy.bindPowerValidationEvent(menuId, num, buttonCode)) {
                $("#faddSave").form("submit", {
                    url: "../sEnroll/AddEnroll",
                    onSubmit: function () {
                        var validate = $("#faddSave").form("validate");//验证
                        if (validate) {
                            $("#btnSaveAddCo").linkbutton("disable");//禁用按钮
                            $("#btnSaveAdd").linkbutton("disable");//禁用按钮
                        }
                        return validate;
                    },
                    success: function (data) {
                        var result = JSON.parse(data);
                        if (result.IsError === false) {
                            sEnrollsProfessionIDs = result.Message;
                            $("#addsEnroll").dialog("close");//关闭弹窗                            
                            $("#sEnrollBuild").dialog({
                                title: "生成缴费单",
                                width: 680,
                                height: 380,
                                href: "../sEnroll/sEnrollBuild",
                                buttons: "#sEnrollBuild-buttons",
                                modal: true,
                                closable: false
                            });
                        }
                        else {
                            Easy.centerShow("系统消息", result.Message, 3000);
                        }
                        $("#btnSaveAddCo").linkbutton("enable");//解除按钮禁用
                        $("#btnSaveAdd").linkbutton("enable");//解除按钮禁用
                    }
                });
            }
        }
    });
}

tempBindSaveButtonClickEventBybtnCode = function (dialog, form, url, btn, type, loadId, menuId, num, btnCode) {
    $(btn).click(function () {
        if ($(btn).linkbutton("options").disabled === false) {//按钮没有禁用才能执行（easyui特殊处理）
            var buttonCode = btnCode;
            if (Easy.bindPowerValidationEvent(menuId, num, buttonCode)) {
                $(form).form("submit", {
                    url: url,
                    onSubmit: function () {
                        var validate = $(form).form("validate");//验证
                        if (validate) {
                            $("#btnSavesEnrollTurnMajorCo").linkbutton("disable");//禁用按钮
                            $(btn).linkbutton("disable");//禁用按钮
                        }
                        return validate;
                    },
                    success: function (data) {
                        var result = JSON.parse(data);
                        if (result.IsError === false) {
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
                            Easy.centerShow("系统消息", result.Message, 3000);
                        }
                        $("#btnSavesEnrollTurnMajorCo").linkbutton("enable");//解除按钮禁用
                        $(btn).linkbutton("enable");//解除按钮禁用
                    }
                });
            }
        }
    });
}

bindTurnMajorCoButtonClickEvent = function (menuId, num) {
    $("#btnSavesEnrollTurnMajorCo").click(function () {
        if ($("#btnSavesEnrollTurnMajorCo").linkbutton("options").disabled === false) {//按钮没有禁用才能执行（easyui特殊处理）
            var buttonCode = "trunMajor";
            if (Easy.bindPowerValidationEvent(menuId, num, buttonCode)) {
                $("#fSaveMajor").form("submit", {
                    url: "../sEnroll/GetsEnrollMajor",
                    onSubmit: function () {
                        var validate = $("#fSaveMajor").form("validate");//验证
                        if (validate) {
                            $("#btnSavesEnrollTurnMajorCo").linkbutton("disable");//禁用按钮
                            $("#btnSavesEnrollTurnMajor").linkbutton("disable");//禁用按钮
                        }
                        return validate;
                    },
                    success: function (data) {
                        var result = JSON.parse(data);
                        if (result.IsError === false) {
                            sEnrollsProfessionIDs = result.Message;
                            $("#sEnrollTurnMajor").dialog("close");//关闭弹窗                            
                            $("#sEnrollBuild").dialog({
                                title: "生成缴费单",
                                width: 680,
                                height: 380,
                                href: "../sEnroll/sEnrollBuild",
                                buttons: "#sEnrollBuild-buttons",
                                modal: true,
                                closable: false
                            });
                        }
                        else {
                            Easy.centerShow("系统消息", result.Message, 3000);
                        }
                        $("#btnSavesEnrollTurnMajorCo").linkbutton("enable");//解除按钮禁用
                        $("#btnSavesEnrollTurnMajor").linkbutton("enable");//解除按钮禁用
                    }
                });
            }
        }
    });
}

bindSaveUplodMethod = function () {
    $("#btnSaveUpsEnroll").click(function () {
        var data = $("#gridStudent").datagrid("getData");
        if (data.rows.length === 0) {
            Easy.centerShow("系统消息", "没有学生信息", 2000);
            return false;
        }
        Easy.bindSaveUploadFileForm("#fupload", "../sEnroll/UploadsEnroll", "#btnSaveUpsEnroll", "#grid", "1", "upload", "#gridStudent", data.rows, "#stuLayout");
    });
}


modStatus = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "modStatus")) {
        var rows = $("#grid").datagrid("getSelections");
        if (Easy.checkRow(rows, "报名信息")) {

            rowData = rows[0];
            if (rowData.Status != "4" && rowData.Status != "5" && rowData.Status != "6" && rowData.Status != "7") {
                Easy.centerShow("系统消息", "该状态不能更改", 2000);
                return false;

            }
            Easy.OpenDialogEvent("#ModifyStatus", "变更在校状态", 400, 300, "../sEnroll/sEnrollModStatus", "#ModifyStatus-buttons");
        }
    }
};

bindSaveModStatusMethod = function () {
    $("#btnSaveModifyStatus").click(function () {
        var id = $("#modsEnrollsProfessionID").textbox("getValue");
        var status = $("#modStatus").combobox("getValue");
        var explain = $("#Explain").textbox("getValue");

        if (status === "" || status === null) {
            Easy.centerShow("系统消息", "请选择要变更的状态", 2000);
            return false;
        }
        if (explain === "" || explain === null) {
            Easy.centerShow("系统消息", "请填写变更状态原因", 2000);
            return false;
        }
        var result = Easy.bindSelectInfomation("../sEnroll/ModifyStatus", {
            Id: id, status: status, explain: explain
        });
        if (result.IsError === false) {
            $("#grid").datagrid("reload");
            $("#ModifyStatus").dialog("close");
            Easy.centerShow("系统消息", result.Message, 2000);

        }
        else {
            Easy.centerShow("系统消息", result.Message, 2000);
        }
    });
};

upModStatus = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "upModStatus")) {
        Easy.OpenDialogEvent("#UploadStatus", "导入在校状态", 720, 590, "../sEnroll/sEnrollsProfessionStatusUpload", "#UploadStatus-buttons");
    }
}

bindSaveUplodMethod = function () {
    $("#btnSaveUpsEnroll").click(function () {
        var data = $("#gridStudent").datagrid("getData");
        if (data.rows.length === 0) {
            Easy.centerShow("系统消息", "没有学生信息", 2000);
            return false;
        }
        Easy.bindSaveUploadFileForm("#fupload", "../sEnroll/UploadsEnroll", "#btnSaveUpsEnroll", "#grid", "1", "upload", "#gridStudent", data.rows, "#stuLayout");
    });
};

downModStatus = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "downModStatus")) {
        location.href = "../ModelExcel/学生在校状态变更模板.xls";
    }
}
//保存上传修改状态
bindSaveUplodModifyStatusMethod = function () {
    $("#btnSaveUploadStatus").click(function () {
        var data = $("#gridsEnroll").datagrid("getData");
        if (data.rows.length === 0) {
            Easy.centerShow("系统消息", "在校状态变更信息", 2000);
            return false;
        }
        Easy.bindSaveUploadFileForm("#fchaUpload", "../sEnrollsProfessionStatusUpload/UploadsChangesEnrollsProfessionStatus", "#btnSaveUploadStatus", "#grid", "1", "upModStatus", "#gridsEnroll", data.rows, "#chaLayout");
    });
};
//修改班级
upClassgrouping = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "upClassgrouping")) {
        Easy.OpenDialogEvent("#ClassGrouping", "导入班级信息", 720, 590, "../sEnroll/ClassGrouping", "#ClassGrouping-buttons");
    }
};
//导出修改班级模板
downClassgrouping = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "downClassgrouping")) {
        location.href = "../ModelExcel/学生分班级模板.xls";
    }
};
//保存修改班级
bindSaveClassgroupMethod = function () {
    $("#btnSaveClassGrouping").click(function () {
        var data = $("#gridClassgroup").datagrid("getData");
        if (data.rows.length === 0) {
            Easy.centerShow("系统消息", "学生分班信息", 2000);
            return false;
        }
        Easy.bindSaveUploadFileForm("#fgroupUpload", "../ClassGrouping/SaveClassGrouping", "#btnSaveClassGrouping", "#grid", "1", "upClassgrouping", "#gridClassgroup", data.rows, "#groupLayout");
    });
}
//修改班级
editClass = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "editClass")) {
        var rows = $("#grid").datagrid("getSelections");
        if (Easy.checkRow(rows, "报名信息")) {
            rowData = rows[0];
            Easy.OpenDialogEvent("#ModfiyClass", "修改班级", 400, 300, "../sEnroll/ModClass", "#ModfiyClass-buttons");
        }
    }
};
//保存修改班级
bindSavaModifyClassMetod = function () {
    $("#btnSaveModfiyClass").click(function () {
        var id = $("#modcsEnrollsProfessionID").textbox("getValue");
        var classId = $("#modcClass").combobox("getValue");
        if (classId === "" || classId === null) {
            Easy.centerShow("系统消息", "请选择班级", 2000);
        }
        var result = Easy.bindSelectInfomation("../sEnroll/ModifyClass", {
            Id: id, classId: classId
        });
        if (result.IsError === false) {
            $("#grid").datagrid("reload");
            $("#ModfiyClass").dialog("close");
            Easy.centerShow("系统消息", result.Message, 2000);

        }
        else {
            Easy.centerShow("系统消息", result.Message, 2000);
        }
    });
};

editLeaveYear = function () {

    if (Easy.bindPowerValidationEvent(menuId, "1", "editLeaveYear")) {
        var rows = $("#grid").datagrid("getSelections");
        if (Easy.checkRow(rows, "报名信息")) {
            rowData = rows[0];
            Easy.OpenDialogEvent("#ModifyLeaveYear", "修改离校年度", 400, 300, "../sEnroll/ChangeLeaveYear", "#ModifyLeaveYear-buttons");
        }
    }
};
bindSavaModifyLeaveYearMethod = function () {
    $("#btnSaveModifyLeaveYear").click(function () {
        var id = $("#modlsEnrollsProfessionID").textbox("getValue");
        var leaveYear = $("#modlLeave").combobox("getValue");
        if (leaveYear === "" || leaveYear === null) {
            Easy.centerShow("系统消息", "请选择班级", 2000);
        }
        var result = Easy.bindSelectInfomation("../sEnroll/ModifyLeaveYear", { Id: id, leaveYear: leaveYear });
        if (result.IsError === false) {
            $("#grid").datagrid("reload");
            $("#ModifyLeaveYear").dialog("close");
            Easy.centerShow("系统消息", result.Message, 2000);

        }
        else {
            Easy.centerShow("系统消息", result.Message, 2000);
        }
    });
};

upLeaveYear = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "upLeaveYear")) {
        Easy.OpenDialogEvent("#upLeaveYear", "导入毕业年份", 700, 550, "../sEnroll/UploadLeaveYear", "#upLeaveYear-buttons");
    }
};
deriveLeaveYearTemplate = function () {

    if (Easy.bindPowerValidationEvent(menuId, "1", "deriveLeaveYearTemplate")) {
        location.href = "../ModelExcel/学生毕业年份模板.xls";
    }
};

bindSaveUplodModifyLeaveYearMethod = function () {
    $("#btnSaveupLeaveYear").click(function () {
        var data = $("#gridInfo").datagrid("getData");
        if (data.rows.length === 0) {
            Easy.centerShow("系统消息", "毕业年份信息", 2000);
            return false;
        }
        Easy.bindSaveUploadFileForm("#fupLeaveYearForm", "../EnrollChangeLeaveYear/UploadChageLeaveYear", "#btnSaveupLeaveYear", "#grid", "1", "upLeaveYear", "#gridInfo", data.rows, "#upLeaveYearLayout");
    });
};

changeTurnOfficial = function () {
    var rows = $("#grid").datagrid("getSelections");
    if (Easy.checkRow(rows, "报名信息")) {
        if (rows[0].Status === "2" || rows[0].Status === "3" || rows[0].Status === "4") {
            Easy.OpenDialogEvent("#changeTurn", "变更转证报", 400, 300, "../sEnroll/ChangeTurnOfficial", "#changeTurn-buttons");
        }
        else {
            Easy.centerShow("系统消息", "只有预报名、正式报名、在校才能进行此项操作！", 2000);
        }
    }
}
bindSavaTurnOfficial = function () {
    $("#btnSavechangeTurn").click(function () {
        var rows = $("#grid").datagrid("getSelections");
        if (Easy.checkRow(rows, "报名信息")) {
            var id = rows[0].sEnrollsProfessionID;
            var type = $("#ChangeType").combobox("getValue");
            if (type === "") {
                Easy.centerShow("系统消息", "请选择类型", 2000);
                return false;
            }
            if (rows[0].Status === "2" && (type === "2" || type === "3")) {
                Easy.centerShow("系统消息", "预报名状态不能选择此类型", 2000);
                return false;
            }
            if ((rows[0].Status === "3" || rows[0].Status === "4") && type === "1") {
                Easy.centerShow("系统消息", "正式报名状态不能选择此类型", 2000);
                return false;
            }

            var result = Easy.bindSelectInfomation("../sEnroll/SaveChangeTrun", { Id: id, Type: type });
            if (result.IsError === false) {
                $("#grid").datagrid("reload");
                $('#changeTurn').dialog('close');
                Easy.centerShow("系统消息", result.Message, 2000);
                return;
            }
            else {
                Easy.centerShow("系统消息", result.Message, 2000);
                return;
            }
        }
    });
}

enrollRefund = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "enrollRefund")) {
        var rows = $("#grid").datagrid("getSelections");
        if (Easy.checkRow(rows, "报名信息")) {
            if (rows[0].Status != "2") {
                Easy.centerShow("系统消息", "只有预报名的学生才能进行此项操作！", 2000);
                return false;
            }
            Easy.OpenDialogEvent("#senrollRefund", "" + rows[0].StudentName + " 退费", 400, 200, "../sEnroll/sEnrollRefund", "#senrollRefund-buttons");

        }
    }
}
bindSaveEnrollRefund = function () {
    $("#btnSavesenrollRefund").click(function () {
        var rows = $("#grid").datagrid("getSelections");
        if (Easy.checkRow(rows, "报名信息")) {

            var remark = $("#RefundRemark").textbox("getValue");
            if (remark === "") {
                Easy.centerShow("系统消息", "退费原因不能为空！", 2000);
                return false;
            }
            var result = Easy.bindSelectInfomation("../sEnroll/UpdateStatus", { ID: rows[0].sEnrollsProfessionID, Remark: remark });
            if (result.IsError === false) {
                $("#grid").datagrid("reload");
                $("#senrollRefund").dialog('close');
                Easy.centerShow("系统消息", result.Message, 2000);
            }
            else {
                Easy.centerShow("系统消息", result.Message, 2000);
            }
        }
    });
}

upEnrollDept = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "upEnrollDept")) {
        Easy.OpenDialogEvent("#upEnrollDept", "导入报名校区", 700, 550, "../sEnroll/UploadEnrollDept", "#upEnrollDept-buttons");
    }
};
downEnrollDeptTemplate = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "downEnrollDeptTemplate")) {
        location.href = "../ModelExcel/报名校区模板.xls";
    }
}

bindSaveUplodEnrollDeptMethod = function () {
    $("#btnSaveupEnrollDept").click(function () {
        var data = $("#gridEnrollDeptInfo").datagrid("getData");
        if (data.rows.length === 0) {
            Easy.centerShow("系统消息", "报名校区信息", 2000);
            return false;
        }
        Easy.bindSaveUploadFileForm("#f_enrollForm", "../sEnroll/GetUploadEnrollDept", "#btnSaveupEnrollDept", "#grid", "1", "upEnrollDept", "#gridEnrollDeptInfo", data.rows, "#upenrollLaout");
    });
};
upStudyNum = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "upStudyNum")) {
        Easy.OpenDialogEvent("#upStudyNum", "导入学号", 700, 550, "../sEnroll/UploadStudyNum", "#upStudyNum-buttons");
    }
}
downupStudyNumTemplate = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "downupStudyNumTemplate")) {
        location.href = "../ModelExcel/新学号导入模板.xls";
    }
}
bindSaveUploadStudyNum = function () {
    $("#btnSaveupStudyNum").click(function () {
        var data = $("#gridStudyNum").datagrid("getData");
        if (data.rows.length === 0) {
            Easy.centerShow("系统消息", "报名校区信息", 2000);
            return false;
        }
        Easy.bindSaveUploadFileForm("#f_studyForm", "../sEnroll/GetUploadStudyNum", "#btnSaveupStudyNum", "#grid", "1", "upStudyNum", "#gridStudyNum", data.rows, "#upStudyNumLayout");
    });
};
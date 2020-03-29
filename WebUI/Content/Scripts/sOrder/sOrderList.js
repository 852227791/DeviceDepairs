firstFunction = function () {
    //加载表格
    initTable({ MenuID: menuId });

    //加载搜索表单数据
    bindSearchFormEvent();

    //加载搜索按钮的点击事件
    bindSearchClickEvent();

    //加载开关高级搜索事件
    Easy.bindOpenCloseSearchBoxEvent(90);

    //点击修改保存按钮
    Easy.bindSaveButtonClickEvent("#sOrderEdit", "#fsOrderSave", "../sOrder/GetsOrderEdit", "#btnSavesOrderEdit", "1", "#grid", menuId, "1", "#sOrderID");

    Easy.bindSaveButtonClickEventBybtnCode("#sOrderAdd", "#fsOrderAddSave", "../sOrder/GetsOrderAdd", "#btnSavesOrderAdd", "1", "#grid", menuId, "1", "add");
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
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "sOrderID", checkbox: true },
            { field: "sEnrollsProfessionID", hidden: true },
            { field: "StudentID", hidden: true },
            { field: "StatusName", title: "状态", width: 60, sortable: true },
            { field: "DeptName", title: "校区", width: 200, sortable: true },
            { field: "StuName", title: "学生姓名", width: 80, sortable: true },
            { field: "EnrollNum", title: "学号", width: 100, sortable: true },
            { field: "IDCard", title: "身份证号", width: 140, sortable: true },
            { field: "Year", title: "年份", width: 60, sortable: true },
            { field: "MonthName", title: "月份", width: 80, sortable: true },
            { field: "LevelName", title: "学历层次", width: 80, sortable: true },
            { field: "MajorName", title: "报读专业", width: 120, sortable: true },
            { field: "EnrollTime", title: "报名时间", width: 80, sortable: true },
            { field: "PlanName", title: "缴费方案", width: 120, sortable: true },
            { field: "NumName", title: "缴费学年", width: 100, sortable: true },
            { field: "FeeType", title: "收费类别", width: 80, sortable: true },
            { field: "FeeName", title: "费用类别", width: 80, sortable: true },
            { field: "LimitTime", title: "供贷日期", width: 90, sortable: true },
            { field: "ShouldMoney", title: "应供贷金额", sortable: true, halign: 'left', align: 'right' },
            { field: "PaidMoney", title: "已供贷金额", sortable: true, halign: 'left', align: 'right' },
            { field: "OwnMoney", title: "未供贷金额", sortable: true, halign: 'left', align: 'right' },
            { field: "CreateTime", title: "创建时间", width: 90, sortable: true }
        ]],
        url: "../sOrder/GetsOrderInfoList", sortName: "sOrderID", sortOrder: "asc",
        toolbar: Easy.loadToolbar(menuId, "1"),
        rowStyler: function (index, row) {
            if (row.Status === "9") {
                return "color:#ff0000;";
            }
            else if (row.Status === "3") {
                return "color:#339900;";
            }
        },
        onLoadSuccess: function (data) {
            $('#grid').resizeDataGrid(0, 0, 0, 0);//重新计算宽度高度
        }
    });
}

//绑定搜索按钮的点击事件
bindSearchClickEvent = function () {
    $("#btnSearch").click(function () {//按条件进行查询数据，首先我们得到数据的值
        var queryData = rerurnQueryData();
        initTable(queryData);//将值传递给
    });
    $("#btnReset").click(function () {
        $("#fSearch").form("reset");//重置表单
        initTable({ MenuID: menuId });
    });
}

//绑定搜索表单数据
bindSearchFormEvent = function () {
    $("#treeDept").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "No", Status: "1" },
        animate: true,
        lines: true,
        panelWidth: 300
    });

    $("#selLevel").combobox({
        url: "../Refe/SelList?RefeTypeID=17",
        valueField: "Value",
        textField: "RefeName",
        multiple: true
    });

    $("#selStatus").combobox({
        url: "../Refe/SelList?RefeTypeID=19",
        valueField: "Value",
        textField: "RefeName",
        multiple: true
    });

    $("#selKindof").combobox({
        url: "../Refe/SelList?RefeTypeID=15",
        valueField: "Value",
        textField: "RefeName",
        multiple: true
    });
}

//添加
add = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "add")) {
        Easy.OpenDialogEvent("#sOrderAdd", "添加缴费单信息", 380, 280, "../sOrder/sOrderAdd", "#sOrderAdd-buttons");
    }
}

//修改
edit = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "edit")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "缴费单信息")) {
            var result1 = Easy.bindSelectInfo("../sOrder/SelectsOrder", rows[0].sOrderID);
            var sorder = JSON.parse(result1.Message)[0];
            if (parseFloat(sorder.PaidMoney) > 0) {
                Easy.centerShow("系统消息", "已缴费的项不能修改", 3000);
            }
            else {
                Easy.OpenDialogEvent("#sOrderEdit", "编辑缴费单信息", 360, 220, "../sOrder/sOrderEdit", "#sOrderEdit-buttons");
            }
        }
    }
}

//停用
disable = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "disable")) {
        bindUpdateStatus("停用", "9");
    }
}

//查看
view = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "view")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "缴费单信息")) {
            Easy.OpenDialogEvent("#sOrderDetail", "查看缴费单信息", 800, 600, "../sOrder/sOrderDetail", "#sOrderDetail-buttons");
        }
    }
}

//转欠费
trunownfee = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "trunownfee")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        //验证是否选中行
        if (Easy.checkRows(rows, "缴费单信息")) {
            var verify = false;
            var rowsIdStr = "";
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].FeeType != "资助") {
                    verify = true;
                    break;
                }
                rowsIdStr += rows[i].sOrderID + ",";
            }
            if (verify) {
                Easy.centerShow("系统消息", "只有资助才能转欠费", 3000);
            }
            else {
                if (rowsIdStr != "") {
                    rowsIdStr = rowsIdStr.substring(0, rowsIdStr.length - 1);
                }
                //修改费用类别
                Easy.bindUpdateValues("转欠费", "1", "../sOrder/GetTrunOwnFee", rowsIdStr, "1", "#grid");
            }
        }
    }
}

//导出
derive = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "derive")) {
        var queryData = rerurnQueryData();
        $.ajax({
            type: "post",
            url: "../sOrder/DownloadsOrder",
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

bindUpdateStatus = function (confirmstr, status) {
    var rows = $("#grid").datagrid("getSelections");//选中的所有ID
    //验证是否选中行
    if (Easy.checkRows(rows, "缴费单信息")) {
        var rowsIdStr = "";
        for (var i = 0; i < rows.length; i++) {
            rowsIdStr += rows[i].sOrderID + ",";
        }
        if (rowsIdStr != "") {
            rowsIdStr = rowsIdStr.substring(0, rowsIdStr.length - 1);
        }
        //修改状态
        Easy.bindUpdateValues(confirmstr, status, "../sOrder/GetUpdatesStatus", rowsIdStr, "1", "#grid");
    }
}

rerurnQueryData = function () {
    //得到用户输入的参数
    var queryData = {
        MenuID: menuId,
        treeDept: $("#treeDept").combotree("getValue"),
        txtStuName: $("#txtStuName").textbox("getValue"),
        txtEnrollNum: $("#txtEnrollNum").textbox("getValue"),
        txtIDCard: $("#txtIDCard").textbox("getValue"),
        txtSignTimeS: $("#txtSignTimeS").datebox("getValue"),
        txtSignTimeE: $("#txtSignTimeE").datebox("getValue"),
        selLevel: "" + $("#selLevel").combobox("getValues"),
        txtMajor: $("#txtMajor").textbox("getValue"),
        txtYear: $("#txtYear").textbox("getValue"),
        txtMonth: $("#txtMonth").textbox("getValue"),
        txtPlanName: $("#txtPlanName").textbox("getValue"),
        txtNumName: $("#txtNumName").textbox("getValue"),
        txtFeeName: $("#txtFeeName").textbox("getValue"),
        txtLimitTimeS: $("#txtLimitTimeS").textbox("getValue"),
        txtLimitTimeE: $("#txtLimitTimeE").textbox("getValue"),
        selStatus: "" + $("#selStatus").combobox("getValues"),
        selKindof: "" + $("#selKindof").combobox("getValues")
    }
    return queryData;
}



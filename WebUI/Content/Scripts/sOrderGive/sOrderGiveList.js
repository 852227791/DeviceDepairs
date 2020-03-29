firstFunction = function () {
    //加载表格
    initTable({ MenuID: menuId });

    //加载搜索表单数据
    bindSearchFormEvent();

    //加载搜索按钮的点击事件
    bindSearchClickEvent();

    //加载开关高级搜索事件
    Easy.bindOpenCloseSearchBoxEvent(90);
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
            { field: "sOrderGiveID", checkbox: true },
            { field: "StatusName", title: "状态", width: 60, sortable: true },
            { field: "DeptName", title: "校区", width: 200, sortable: true },
            { field: "StuName", title: "学生姓名", width: 80, sortable: true },
            { field: "EnrollNum", title: "学号", width: 100, sortable: true },
            { field: "IDCard", title: "身份证号", width: 140, sortable: true },
            { field: "Year", title: "年份", width: 60, sortable: true },
            { field: "MonthName", title: "月份", width: 80, sortable: true },
            { field: "LevelName", title: "学习层次", width: 80, sortable: true },
            { field: "MajorName", title: "专业", width: 120, sortable: true },
            { field: "EnrollTime", title: "报名时间", width: 80, sortable: true },
            { field: "PlanName", title: "缴费方案名称", width: 140, sortable: true },
            { field: "GiveName", title: "配品名称", width: 100, sortable: true },
            { field: "CreateTime", title: "创建时间", width: 90, sortable: true }
        ]],
        url: "../sOrderGive/GetsOrderGiveInfoList", sortName: "sOrderGiveID", sortOrder: "desc",
        toolbar: Easy.loadToolbar(menuId, "1"),
        rowStyler: function (index, row) {
            if (row.Status === "9") {
                return "color:#ff0000;";
            }
            else if (row.Status === "2") {
                return "color:#339900;";
            }
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
        url: "../Refe/SelList?RefeTypeID=20",
        valueField: "Value",
        textField: "RefeName",
        multiple: true
    });
}

//停用
disable = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "disable")) {
        bindUpdateStatus("停用", "9");
    }
}

//导出
derive = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "derive")) {
        var queryData = rerurnQueryData();
        $.ajax({
            type: "post",
            url: "../sOrderGive/DownloadsOrderGive",
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
    if (Easy.checkRows(rows, "配品信息")) {
        var rowsIdStr = "";
        for (var i = 0; i < rows.length; i++) {
            rowsIdStr += rows[i].sOrderGiveID + ",";
        }
        if (rowsIdStr != "") {
            rowsIdStr = rowsIdStr.substring(0, rowsIdStr.length - 1);
        }
        //修改状态
        Easy.bindUpdateValues(confirmstr, status, "../sOrderGive/GetUpdatesStatus", rowsIdStr, "1", "#grid");
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
        txtGiveName: $("#txtGiveName").textbox("getValue"),
        selStatus: "" + $("#selStatus").combobox("getValues")
    }
    return queryData;
}



firstFunction = function () {
    //加载表格
    initTable({ MenuID: menuId });

    //加载搜索表单数据
    bindSearchFormEvent();

    //加载搜索按钮的点击事件
    bindSearchClickEvent();

    //加载开关高级搜索事件
    Easy.bindOpenCloseSearchBoxEvent(90);

    //批量添加保存方法
    bindsOrderAddClick();
}

//加载表格
initTable = function (queryData) {
    $("#grid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        showFooter: true,
        pageSize: 20,
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "sOrderAddID", checkbox: true },
            { field: "StatusName", title: "状态", width: 60, sortable: true },
            { field: "Name", title: "名称", width: 100, sortable: true },
            { field: "DeptName", title: "校区", width: 200, sortable: true },
            { field: "Year", title: "年份", width: 60, sortable: true },
            { field: "Month", title: "月份", width: 80, sortable: true },
            { field: "Major", title: "专业", width: 200, sortable: true },
            { field: "PlanName", title: "缴费方案名称", width: 120, sortable: true },
            { field: "NumName", title: "缴费次数名称", width: 100, sortable: true },
            //{ field: "Remark", title: "备注", width: 200, sortable: true },
            { field: "CreateName", title: "创建人", width: 80, sortable: true },
            { field: "CreateTime", title: "创建时间", width: 90, sortable: true }
        ]],
        url: "../sOrderAdd/GetsOrderAddInfo", sortName: "sOrderAddID", sortOrder: "desc",
        toolbar: Easy.loadToolbar(menuId, "1"),
        rowStyler: function (index, row) {
            if (row.Status === "9") {
                return "color:#ff0000;";
            }
            else if (row.Status === "1") {
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

    $("#selStatus").combobox({
        url: "../Refe/SelList?RefeTypeID=23",
        valueField: "Value",
        textField: "RefeName",
        multiple: true
    });

    $("#selYear").combobox({
        url: "../Common/YearCombobox?Year=2010&IsSelected=No&Is=No",
        valueField: "Id",
        textField: "Text",
        panelHeight: 120,
        multiple: true
    });

    $("#selMonth").combobox({
        url: "../Refe/SelList?RefeTypeID=18",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 120,
        multiple: true
    });
}

//添加
add = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "add")) {
        Easy.OpenDialogEvent("#sOrderAddMore", "批量添加缴费单", 660, 340, "../sOrderAdd/sOrderAdd", "#sOrderAddMore-buttons");
    }
}

//撤销
reback = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "reback")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "批量缴费单信息")) {
            $.messager.confirm("", "确定要撤销吗？", function (c) {
                if (c) {
                    $.ajax({
                        type: "post",
                        url: "../sOrderAdd/GetReback",
                        async: false,
                        data: { ID: rows[0].sOrderAddID },
                        dataType: "text",
                        success: function (data) {
                            if (data != null && data != "") {
                                Easy.centerShow("系统消息", data, 0);
                                $("#grid").datagrid("load");//刷新表格
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
}

//查看
view = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "view")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "批量缴费单信息")) {
            Easy.OpenDialogEvent("#sOrderAddView", "查看批量缴费单信息", 680, 380, "../sOrderAdd/sOrderAddView", "#sOrderAddView-buttons");
        }
    }
}

rerurnQueryData = function () {
    //得到用户输入的参数
    var queryData = {
        MenuID: menuId,
        txtName: $("#txtName").textbox("getValue"),
        treeDept: $("#treeDept").combotree("getValue"),
        selYear: "" + $("#selYear").combobox("getValues"),
        selMonth: "" + $("#selMonth").combobox("getValues"),
        //txtMajor: $("#txtMajor").textbox("getValue"),
        txtPlanName: $("#txtPlanName").textbox("getValue"),
        txtNumName: $("#txtNumName").textbox("getValue"),
        txtCreateName: $("#txtCreateName").textbox("getValue"),
        txtCreateTimeS: $("#txtCreateTimeS").datebox("getValue"),
        txtCreateTimeE: $("#txtCreateTimeE").datebox("getValue"),
        selStatus: "" + $("#selStatus").combobox("getValues")
    }
    return queryData;
}

bindsOrderAddClick = function () {
    $("#btnSavesOrderAddMore").click(function () {
        if (Easy.bindPowerValidationEvent(menuId, "1", "add")) {
            var validate = $("#fsOrderAddMoreSave").form("validate");//页面验证
            if (validate) {
                var sOrderAddQueryData = rerurnsOrderAddQueryData();
                //批量添加
                $.messager.confirm("", "确定要批量添加缴费单吗？", function (c) {
                    if (c) {
                        $.ajax({
                            type: "post",
                            url: "../sOrderAdd/GetsOrderAdd",
                            async: false,
                            data: sOrderAddQueryData,
                            dataType: "text",
                            success: function (data) {
                                if (data == "yes") {
                                    $('#sOrderAddMore').dialog('close');
                                    $("#sOrderAddResult").dialog({
                                        title: "批量添加缴费单",
                                        width: 680,
                                        height: 380,
                                        href: "../sOrderAdd/sOrderAddResult",
                                        buttons: "#sOrderAddResult-buttons",
                                        modal: true,
                                        closable: false
                                    });
                                    $("#grid").datagrid("load");//刷新表格
                                }
                                else {
                                    Easy.centerShow("系统消息", data, 3000);
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

rerurnsOrderAddQueryData = function () {
    var queryData = {
        sOrderContent: $("#sOrderContent").textbox("getValue"),
        Name: $("#Name").textbox("getValue"),
        DeptID: $("#DeptID").combotree("getValue"),
        Year: $("#Year").combobox("getValue"),
        Month: $("#Month").combobox("getValue"),
        sProfessionID: $("#sProfessionID").combobox("getValues").toString(),
        Scheme: $("#Scheme").combobox("getValue"),
        Semester: $("#Semester").combobox("getValue"),
        Remark: $("#Remark").textbox("getValue")
    }
    return queryData;
}



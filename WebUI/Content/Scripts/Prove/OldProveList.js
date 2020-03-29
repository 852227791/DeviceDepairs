var ProveID = "0";//定义ID
firstFunction = function () {
    //加载表格
    initTable({ MenuID: menuId });

    //加载搜索表单数据
    bindSearchFormEvent();

    //加载搜索按钮的点击事件
    bindSearchClickEvent();

    //加载开关高级搜索事件
    Easy.bindOpenCloseSearchBoxEvent(90);

    //点击添加保存按钮
    Easy.bindSaveUploadFile("#upProve", "#fupOldProve", "../Prove/UploadOldProve", "#btnupProve", "1", "#grid", menuId, "1", "uploadOldProve");
}

//加载表格
initTable = function (queryData) {
    $("#grid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "ProveID", hidden: true },
            { field: "DeptName", title: "校区", width: 140, sortable: true },
            { field: "ProfessionName", title: "专业", width: 140, sortable: true },
            { field: "ClassName", title: "班级", width: 120, sortable: true },
            { field: "StudentName", title: "学生姓名", width: 80, sortable: true },
            { field: "IDCard", title: "身份证号", width: 140, sortable: true },
            { field: "Mobile", title: "手机号", width: 100, sortable: true },
            { field: "ItemName", title: "证书名称", width: 140, sortable: true },
            { field: "EnrollTime", title: "报名时间", width: 130, sortable: true },
            { field: "Status", title: "状态", width: 90, sortable: true }
        ]],
        url: "../Prove/GetProveOldList", sortName: "ProveID", sortOrder: "desc",
        toolbar: Easy.loadToolbar(menuId, "1"),
        rowStyler: function (index, row) {
            if (row.StatusValue === "9") {
                return "color:#ff0000;";
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
    $("#selStatus").combobox({
        url: "../Refe/SelList?RefeTypeID=9",
        valueField: "Value",
        textField: "RefeName",
        panelWidth: 100,
        multiple: true
    });
    $("#treeDept").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "No", Status: "1" },
        animate: true,
        lines: true,
        panelWidth: 200
    });
    $("#txtEnrollTimeS").datebox({});
    $("#txtEnrollTimeE").datebox({});
}

rerurnQueryData = function () {
    var queryData = {//得到用户输入的参数
        MenuID: menuId,
        txtProveName: $("#txtProveName").val(),
        txtName: $("#txtName").val(),
        txtIDCard: $("#txtIDCard").val(),
        selStatus: "" + $("#selStatus").combobox("getValues"),
        treeDept: $("#treeDept").combotree("getValue"),
        txtEnrollTimeS: $("#txtEnrollTimeS").datebox("getValue"),
        txtEnrollTimeE: $("#txtEnrollTimeE").datebox("getValue")
    }
    return queryData;
}






//查看


uploadOldProve = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "uploadOldProve")) {
        Easy.OpenDialogEvent("#upProve", "导入老证书信息", 400, 300, "../Prove/ProveOldUp", "#upProve-buttons");
    }
}

deriveOldTemplate = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "deriveOldTemplate")) {
        location.href = "../ModelExcel/老系统证书信息导入模板.xls";
    }
}
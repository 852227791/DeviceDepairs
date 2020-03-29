//绑定表单数据
initsOrderAddGrid = function () {
    $("#sOrderGrid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: false,//分页
        pageSize: 20,
        columns: [[
            { field: "Result", title: "添加结果", width: 60, sortable: true },
            { field: "Remark", title: "备注", width: 120, sortable: true },
            { field: "StuName", title: "学生姓名", width: 60, sortable: true },
            { field: "IDCard", title: "身份证号", width: 150, sortable: true },
            { field: "Scheme", title: "缴费方案", width: 150, sortable: true },
            { field: "Semester", title: "缴费次数", width: 100, sortable: true },
            { field: "DetailName", title: "缴费项", width: 80, sortable: true }
        ]],
        toolbar: [{
            iconCls: 'icon-down',
            text: '导出',
            handler: GoOutResultExcel
        }],
        rowStyler: function (index, row) {
            if (row.Result == "失败") {
                return 'color:red;';
            }
        }
    });
}

initFunction = function () {
    $("#titleTips").html("正在批量添加缴费单...，请勿关闭浏览器！");
    $('#btnsOrderAddResult').linkbutton('disable');
    var postData = rerurnsOrderAddQueryData();
    $.ajax({
        type: "post",
        url: "../sOrderAdd/GetsOrderAddResult",
        async: true,
        data: postData,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                var array = JSON.parse(data.Message);
                for (var i = 0; i < array.length; i++) {
                    $('#sOrderGrid').datagrid('appendRow', {
                        Result: array[i].Result,
                        Remark: array[i].Remark,
                        StuName: array[i].StuName,
                        IDCard: array[i].IDCard,
                        Scheme: array[i].Scheme,
                        Semester: array[i].Semester,
                        DetailName: array[i].DetailName
                    });
                }
            }
        },
        error: function () {
            Easy.centerShow("系统消息", "出现未知错误，请联系管理员", 3000);
        }
    });
    //执行完毕后改变提示信息，恢复关闭按钮
    $("#titleTips").html("");
    $("#btnsOrderAddResult").linkbutton('enable');
}

GoOutResultExcel = function () {
    Easy.DeriveFile("#sOrderGrid", "批量添加缴费单结果.xls");
}

initsOrderAddGrid();

initFunction();
var fee = null;
var printfee = null;

printinitTable = function (queryData) {
    fee = $("#printgrid1").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: false,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "iFeeID", checkbox: true },
            { field: "VoucherNum", title: "凭证号", width: 120, sortable: true },
            { field: "Dept", title: "校区", width: 200, sortable: true },
            { field: "DeptAreaName", title: "收费校区", width: 80, sortable: true },
            { field: "Name", title: "缴费人", width: 60, sortable: true },
            { field: "IDCard", title: "身份证号", width: 140, sortable: true },
            { field: "NoteNum", title: "票据号", width: 80, sortable: true },
            { field: "FeeContent", title: "收费项目", width: 80, sortable: true },
            { field: "FeeTime", title: "收费时间", width: 80, sortable: true },
            { field: "FeeMode", title: "缴费方式", width: 60, sortable: true },
            { field: "ShouldMoney", title: "应缴金额", sortable: true, halign: 'left', align: 'right' },
            { field: "PaidMoney", title: "实缴金额", sortable: true, halign: 'left', align: 'right' },
            { field: "DiscountMoney", title: "优惠金额", sortable: true, halign: 'left', align: 'right' },
            {
                field: "OffsetMoney", title: "冲抵金额", sortable: true, halign: 'left', align: 'right',
                formatter: function (value, row, index) {
                    if (row.OffsetStr != "" && row.OffsetStr != null) {
                        return "<span title=\"" + row.OffsetStr.replace(/,/g, "<br />") + "\" class=\"tip\" style=\"text-decoration: underline;cursor: pointer;\">" + value + "</span>";
                    }
                    else {
                        return value;
                    }
                }
            },
            {
                field: "BeOffsetMoney", title: "被冲抵金额", sortable: true, halign: 'left', align: 'right',
                formatter: function (value, row, index) {
                    if (row.BeOffsetStr != "" && row.BeOffsetStr != null) {
                        return "<span title=\"" + row.BeOffsetStr.replace(/,/g, "<br />") + "\" class=\"tip\" style=\"text-decoration: underline;cursor: pointer;\">" + value + "</span>";
                    }
                    else {
                        return value;
                    }
                }
            },
            { field: "RefundMoney", title: "核销金额", sortable: true, halign: 'left', align: 'right' },
            { field: "CreateName", title: "收费人", width: 60, sortable: true },
            { field: "PersonSort", title: "交款人员", width: 60, sortable: true },
            { field: "Status", title: "状态", width: 60, sortable: true },
            { field: "Affirm", title: "结账人", width: 60, sortable: true },
            { field: "AffirmTime", title: "结账时间", width: 80, sortable: true }
        ]],
        url: "../iFee/GetiFeeList", sortName: "iFeeID", sortOrder: "desc",
        rowStyler: function (index, row) {
            if (row.StatusValue === "9") {
                return "color:#ff0000;";
            }
            else if (row.StatusValue === "2") {
                return "color:#339900;";
            }
        },
        onLoadSuccess: function (data) {
            Easy.bindCustomPromptToTableEvent(".tip");
        }
    });
}
printinitTable({ MenuID: menuId });
bindSearchClickEvent = function () {
    $("#btnprintSearch").click(function () {//按条件进行查询数据，首先我们得到数据的值
        var queryData = getSearch();
        printinitTable(queryData);//将值传递给
    });
    $("#btnprintReset").click(function () {
        $("#fprintSearch").form("reset");//重置表单
        printinitTable({ MenuID: menuId });
    });
}
bindSearchClickEvent();

getSearch = function () {
    var queryData = {//得到用户输入的参数
        MenuID: menuId,
        txtFeeTimeS: $("#txtprintFeeTimeS").datebox("getValue"),
        txtFeeTimeE: $("#txtprintFeeTimeE").datebox("getValue"),
        txtClass: $("#txtprintClass").val(),
        txtFeeName: $("#txtPrintFeeName").val()
    }
    return queryData;
}

initPrintFeeTable = function () {
    printfee = $("#printgrid2").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: false,//只允许选择一行
        showFooter: true,
        columns: [[
            { field: "iFeeID", checkbox: true },
            { field: "VoucherNum", title: "凭证号", width: 120, sortable: true },
            { field: "Dept", title: "校区", width: 200, sortable: true },
            { field: "DeptAreaName", title: "收费校区", width: 80, sortable: true },
            { field: "Name", title: "缴费人", width: 60, sortable: true },
            { field: "IDCard", title: "身份证号", width: 140, sortable: true },
            { field: "NoteNum", title: "票据号", width: 80, sortable: true },
            { field: "FeeContent", title: "收费项目", width: 80, sortable: true },
            { field: "FeeTime", title: "收费时间", width: 80, sortable: true },
            { field: "FeeMode", title: "缴费方式", width: 60, sortable: true },
            { field: "ShouldMoney", title: "应缴金额", sortable: true, halign: 'left', width: 80, align: 'right' },
            { field: "PaidMoney", title: "实缴金额", sortable: true, halign: 'left', width: 80, align: 'right' },
            { field: "DiscountMoney", title: "优惠金额", sortable: true, halign: 'left', width: 80, align: 'right' },
            { field: "OffsetMoney", title: "充抵金额", sortable: true, halign: 'left', width: 80, align: 'right' },
            { field: "BeOffsetMoney", title: "被充抵金额", sortable: true, halign: 'left', width: 80, align: 'right' },
            { field: "RefundMoney", title: "核销金额", sortable: true, halign: 'left', width: 80, align: 'right' },
            { field: "CreateName", title: "收费人", width: 60, sortable: true },
            { field: "PersonSort", title: "交款人员", width: 60, sortable: true },
            { field: "Status", title: "状态", width: 60, sortable: true },
            { field: "Affirm", title: "结账人", width: 60, sortable: true },
            { field: "AffirmTime", title: "结账时间", width: 80, sortable: true }
        ]], sortName: "iFeeID", sortOrder: "desc", toolbar: [{
            iconCls: 'icon-add',
            text: '添加',
            handler: function () {

                var rows = fee.datagrid("getSelections");
                if (Easy.checkRows(rows, "杂费收费信息")) {
                    var feeprintData = printfee.datagrid("getData");
                    var feeprintRows = feeprintData.rows;
                    var flag = true
                    var message = "";
                    var money = 0;
                    for (var i = 0; i < rows.length; i++) {
                        if (parseFloat(rows[i].OffsetMoney) > 0) {
                            message = rows[i].VoucherNum + "有充抵记录，不能合并打印";
                            flag = false;
                            break;
                        }
                        if (parseFloat(rows[i].BeOffsetMoney) > 0) {
                            message = rows[i].VoucherNum + "有被充抵记录，不能合并打印";
                            flag = false;
                            break;
                        }
                        for (var k = 0; k < feeprintRows.length; k++) {
                            money = parseFloat(money) + parseFloat(feeprintRows[k].ShouldMoney);

                            if (feeprintRows[k].iFeeID === rows[i].iFeeID) {
                                message = rows[i].VoucherNum + "不能重复添加";
                                flag = false;
                                break;
                            }
                        }
                    }
                    if (!flag) {
                        Easy.centerShow("系统消息", message, 3000);
                        return false;
                    }
                    var temp = 0;

                    for (var i = 0; i < rows.length; i++) {
                        temp = parseFloat(temp) + parseFloat(rows[i].ShouldMoney);
                        printfee.datagrid("insertRow", {
                            index: 0,	// 索引从0开始
                            row: rows[i]
                        })
                    }
                    printfee.datagrid('reloadFooter', [
	                    { name: 'foot', ShouldMoney: "合计:" + ((parseFloat(money) + parseFloat(temp)).toFixed("2")) }
                    ]);

                    fee.datagrid("unselectAll");//取消选择当前页的所行
                }

            }
        }, '-', {
            iconCls: 'icon-no',
            text: '移除',
            handler: function () {
                var rows = printfee.datagrid("getSelections");
                if (Easy.checkRows(rows, "杂费收费信息")) {
                    var foot = printfee.datagrid("getFooterRows");
                    var totalMoney = foot[0].ShouldMoney;
                    totalMoney = totalMoney.substring(totalMoney.indexOf(":") + 1, totalMoney.length);
                    for (var i = 0; i < rows.length; i++) {
                        totalMoney = parseFloat(totalMoney) - parseFloat(rows[i].ShouldMoney);
                        var index = printfee.datagrid("getRowIndex", rows[i]);
                        printfee.datagrid("deleteRow", index);
                    }
                    printfee.datagrid('reloadFooter', [
                      { name: 'foot', ShouldMoney: "合计:" + (totalMoney).toFixed("2") }
                    ]);
                }
            }
        }]

    });
}

initPrintFeeTable();
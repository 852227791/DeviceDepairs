bindFormEvent = function () {
    if (selectRow!=null) {
        $('#fPrintNumSave').form('load', {
            ProveID: selectRow.ProveID,
            ProveNum: selectRow.ProveNum
        });
    }
}
bindFormEvent();
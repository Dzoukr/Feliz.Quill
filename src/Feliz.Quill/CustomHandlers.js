export function imageFromUrl(_) {
    const tooltip = this.quill.theme.tooltip;
    const originalSave = tooltip.save;
    const originalHide = tooltip.hide;
    tooltip.save = function(_) {
        const range = this.quill.getSelection(true);
        const value = this.textbox.value;
        if (value) {
            this.quill.insertEmbed(range.index, 'image', value, 'user');
        }
    };
    tooltip.hide = function(_) {
        tooltip.save = originalSave;
        tooltip.hide = originalHide;
        tooltip.hide();
    };
    tooltip.edit('image');
    tooltip.textbox.placeholder = "Embed URL";
    tooltip.textbox.value = "";
}

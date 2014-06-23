using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Victop.Wpf.Controls
{
    public enum TreeViewControl
    {
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly 标识符的大小写应当正确")]
        tvcCheckBox = 3,
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly 标识符的大小写应当正确")]
        tvcCheckBoxThreeState = 4,
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly 标识符的大小写应当正确")]
        tvcComboBox = 7,
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly 标识符的大小写应当正确")]
        tvcImage = 5,
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly 标识符的大小写应当正确")]
        tvcRadioButton = 6,
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly 标识符的大小写应当正确")]
        tvcTextBlock = 0,
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly 标识符的大小写应当正确")]
        tvcTextBox = 1,
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly 标识符的大小写应当正确")]
        tvcTextBoxRightAlign = 2
    }
}

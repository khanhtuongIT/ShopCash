   M S B A M L   `   `   `  ����  Cash Register2    ,CashierRegister.Pages.Home.SelectSalesperson   5      U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.8.0, Culture=neutral, PublicKeyToken=bc9b0c37bf06c6a90mui&http://firstfloorsoftware.com/ModernUI  5      -   5           list_salesperson  � Title#  �   5	      .��5      &   System.Windows.Controls.DataGrid  -   5          Name$ dtgSalesperson��5      $	��350��6Z   $	��400��6i   $	��600��6y   $��0,10,0,0q�6�      HeadersVisibility$
 None=�6�      AutoGenerateColumns .     AlternatingRowBackground
 ����    
IsReadOnly .    GridLinesVisibility$ 
Horizontal=�6	     Columns 5   
   4  .System.Windows.Controls.DataGridTemplateColumn  5         CellTemplate 5      �� 5      .R�5      � $	��350��6   $��30��6.   	 � ColumnDefinitions	 5      �� 5   "   $	��300��63   6D   �� 5   "   $
��Auto �63   6B   .+�5      �� $��Center=�6(   ��6C   �� 
 �� Path$

 Name�6`   �� 5      -   5        $��1��5   %   $��0q�65   
������ $��Right=�6y   $��Center=�6�   .��5   "   �� $��20��6I   $��20��6T   ��6(   �� $
 ImageUrl�6b    � ToolTip 5   "   �� 5   &    
 select#���  60   6R   5       5      �� 5   "     double_select#���  6,   6U   5!      5"      5$      5&      5'      
   M S B A M L   `   `   `  ����  Cash Register,    &CashierRegister.Pages.Setting.Currency   5      U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.8.0, Culture=neutral, PublicKeyToken=bc9b0c37bf06c6a90mui&http://firstfloorsoftware.com/ModernUI  5      -   5        .��5	      �    ContentRoot#��[  6   $��0q�60   .+�5
   
   =  7FirstFloor.ModernUI.Windows.Controls.ModernProgressRing  -   5           Name$	  mpr��5
   !   $	��100��6.   $	��100��6:   $��Center=�6G   $��Center=�6b      IsActive .    CircleProgressRingStyle#��[ 6�   $��0,0,0,56q�6�   6�   � 5   
   -   5         �Name$ 	grContent��5       � RowDefinitions 5      �� 5      $��45 �6    6.   �� 5      $��* �6    6-   .+�5      � $��0��6   ���	#FFEEEEEE � ColumnDefinitions 5      �� 5      $	��100 �6'   65   �� 5      $��* �6'   63   �� 5      $	��100 �6'   65   .+�5      ��  �� Orientation$ 
Horizontal=�6   $
��Left=�66   $��2,0,0,0q�6Q   .+�5      7  1FirstFloor.ModernUI.Windows.Controls.ModernButton  -   5          Name$ muiBtnDelete��5   '   $��0,0,10,0q�6=      EllipseDiameter$ 35Q�6O      
IconHeight$ 20Q�6d   	   	IconWidth$	 20Q�6t   $��Center=�6�   
   IconData�
 ���)���?
��)���?(�8O"��?(�8O"��?
�8���?
�8���?(��nu��?(��nu��?
���    ����!    ����!Dt����n.Dt����n.��?
�PΎ*��?
�PΎ*��n.�8����n.�8����?
�    ��?
�    Dt����Dt�	 � ToolTip 5      �� 5        delete_currency#���  6(   6S   5        5      -   5        $ 	muiBtnAdd��5   '   $ 35Q�6R   $ 20Q�6g   $	 20Q�6w   $��2��6�   $��Center=�6�   $��Center=�6�   �
 ��.    �㥛� @@@    �㥛� @@@��.Ѱ���.Ѱ����㥛� @@@���㥛� @@@ȉѨ�.ȉѨ�.���    ���    ��.Ѩ�.��.	 5      �� 5        add_currency#���  6(   6P   5!      5"      �� 5#      -   5         ��Name$ tblTotal��5#      $��Center=�6.   
���""��  Heading2#��[ 6b   $��Center=�6�   $��1��6�   6�     5$      -   5        $ muibtnGetCurrent��5$   #   $��Right=�6=   $��0,0,10,0q�6Y   $ 35Q�6k   $ 20Q�6�   $	 20Q�6�   �
 �p�i� lK�V[@S�� �C��@��@�����#@���E�@�Ӱ ���:���!@
�� k� �V"-pE�V"�Cu��V"�
�� #�4�Ӱ S�\�����#@}���C��@���lK�V[@SG�f��L�@�B��=v �B��Ձ ��B��M� +7�H2�w�	0@-pEH2�w�	0@Ҭl�0@H2�w�	0@��� �M� ��� �Ձ Ӈ�� �=v J�� f��L�@�i� lK�V[@�-pE    �a�����@@    i��Ӱ i��Ӱ �i��Ӱ a�����@@m�a-pEm�a����>#@m�a�Ӱ �Ӱ �    �Ӱ ���>#@-pE	$��2��6e   5%      �� 5&        current_currency#���  6$   6P   5(      5)      &   System.Windows.Controls.DataGrid  5+      -   5          Name$ dtgCurrency��5+      $y�Hidden=�6]      
IsReadOnly .    HeadersVisibility$ Column=�6�      AutoGenerateColumns .     SelectionMode$ Single=�6�   $��1��6�      Columns 5,      4  .System.Windows.Controls.DataGridTemplateColumn  5-      ,  &System.Windows.Controls.DataGridColumn   Header 5.      �� 5/      -	   5         ��Name$ chkAll��5/   '   $�� ��67    50   "   �� 51   &     select_all_currency#���  60   6_   53         CellTemplate 55      �� 56      .R�57   "   �� -
   5        $��Center=�57   O   $��4,0,0,0q�6j    d� 	IsChecked 6+   ��  �� Path$ CheckDel� 58   &   �� 59   *     select_this_currency#���  64   6d   5;   #   5<      5>        5K       5L      �� 5M        currency#���  6(   6L    5O      �� 5P      .R�5Q   "   �� $��Center=�6K   ��6,   �� $ Currency�6h   5R      5T        5V       5W      �� 5X       	 tax_rate#��� 	 6(   6L    5Z      �� 5[      .R�5\   "   �� $��Center=�6M   ��6,   �� $ 
StrTaxRate�6j   5]      5_        5a       5b      �� 5c       

 active#��� 
 6(   6J    5e      �� 5f      .R�5g   "   �� {�.   6+   �� $ Active�6a   5h      5j        5l       5m      �� 5n        version#���  6(   6K    5p      �� 5q      .R�5r   "   �� {�.   6+   �� $ Version�6b   5s      5u        5w       5x      �� 5y        edit#���  6(   6H    5{      �� 5|      .R�5}   "   �� -   5        
������ $��Center=�5}   X   $��Center=�6s   
������ $��0q�6�   .��5~   &   �� $��18��6M   $��18��6X   ��6,   �� $ ImageUrl�6f    5   &   �� 5�   *     edit_currency#���  64   6]   5�   #   5�      5�      5�      5�      5�      5�      
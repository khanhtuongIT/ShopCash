   M S B A M L   `   `   `  ����  Cash Register-    'CashierRegister.Pages.Home.FindCustomer   5      U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.8.0, Culture=neutral, PublicKeyToken=bc9b0c37bf06c6a90mui&http://firstfloorsoftware.com/ModernUI  5      -   5        $
��9999��5   <   $
��9999��6M      customer  � Title#  �   5	      .��5      � $	��540��6   $	��Top=�6   $	��440��60    � RowDefinitions 5   
   �� 5      $
��Auto �6   6,   �� 5      $��* �6   6)   �� 5      $��40 �6   6*   �� 5      $��50 �6   6*   .+�5   
   �  5      �� 5      6"   �� 5      6"    � ColumnDefinitions 5      �� 5      $��* �6#   6/   �� 5      $��* �6#   6/   �� 5      $��35 �6#   60   .+�5       ��   customer_name#���  6   $
��Bold"�6?   $��0,0,0,5q�6Q   6d   �� 5!      -   5         ��Name$ txbCustomer��5!      $��Center=�6+   $��25��6F   $��Center=�6R   $��1��6t   $��0,0,10,0q�6�   6�   �� 5#       	 phone#���  6   $��1��67   $
��Bold"�6G   $��0,0,0,5q�6Y   6l   �� 5$      -   5        $ txbPhone��5$      $��Center=�6(   $��25��6C   $��Center=�6O   $��1��6q   $��1��6�   $��0,0,10,0q�6�   6�   7  1FirstFloor.ModernUI.Windows.Controls.ModernButton  5&      -   5          Name$ muiBtnSearch��5&      $��Center=�65   $��2��6P   $��1��6`   $��Right=�6m      EllipseDiameter$ 30Q�6�      
IconHeight$ 15Q�6�      	IconWidth$ 15Q�6�      IconData� ���|h��T��SF0�T��	��Ȯb(#���"��*`@����p�a�q2�l��0"��Ε��Mb��@?��r ��F�H�! |Q�K�K��:��c�h�:UY�)��T�|h��T���O   �8��Y���@X�#�qp��?�!d�+ ��!�Q�`�)��
��(��Ө�*(����&�m��d^$w�`G�1H��+��r�3��-r3�p�0��f1H2Ә�[/���3x�M,�Xi3 j�*��1р�� ��"�Ps��C'$P�м$��HH�g$�(sh]b#lt����m� �bgӄ�,�.Hz
�c  ��O   		 � ToolTip	 5'      �� 5(       
 search#���  6    6B   5*      5+      &   System.Windows.Controls.DataGrid  5-   
   -   5        
  Name$
 dtgCustomer��5-         SelectionMode$ Single=�6(   $	��350��6?   $��0,10,0,0q�6N   $��1��6`      AlternatingRowBackground
 ����    AutoGenerateColumns .     
IsReadOnly .    GridLinesVisibility$ 
Horizontal=�6�      HeadersVisibility$
 None=�6�      Columns 5.      4  .System.Windows.Controls.DataGridTemplateColumn  5/         CellTemplate 50      �� 51      .R�52      � $	��350��6#   $��30��62    53   "   �� 54   &   $	��125��67   6H   �� 55   &   $	��120��67   6H   �� 56   &   $	��125��67   6H   �� 57   &   $
��Auto �67   6F   .+�59   "   �� $��Center=�6K   $��0��6f   ��6,   ��  �� Path$ FullName�6x   �� 5:   "   $��Center=�6H   $��Center=�6c   $��1��6�   ��6,   �� $ Phone�6�   �� 5;   "   $��Right=�6K   $��2��6g   $��Center=�6w   ��6,   �� $ Address1�6�   �� 5<   "   -   5        $��3��5<   A   $��Center=�6Q   $��Right=�6l   
������ $��0q�6�   
������ .��5=   &   �� $��20��6M   $��Center=�6X   $��Center=�6s   $��20��6�   $��3��6�   ��6,   �� $ ImageUrl�6�   	 5>   &   �� 5?   *    
 select#���  64   6V   5A   #   	 5B   "   �� 5C   &     double_select#���  60   6Y   5E      5F      5H      5J      �� 5L   
   -   5         ��Name$ spPaging��5L       �� Orientation$ 
Horizontal=�6'   $��2��6@   $��Center=�6M   $��Center=�6h   $��	0,10,0,10q�6�   5M      �� 5O   
   $ 
Horizontal=�6   $��3��6.   $��Right=�6;   $��Bottom=�6W   .+�5P      �� -   5         ��Name$ 	muiBtnAdd��5P      $��0,0,5,0q�6@   $��80��6Q     add#���  6\   6~   �� 5Q      -	   5        $ muiBtnClose��5Q      $��80��6D    	 close#���  6O   6s   5R      5T      5U      
   M S B A M L   `   `   `  ����  Cash Register/    )CashierRegister.Pages.Setting.ExportLocal   5      U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.8.0, Culture=neutral, PublicKeyToken=bc9b0c37bf06c6a90mui&http://firstfloorsoftware.com/ModernUI  5      .��5	      �    ContentRoot#��[  6   $��0q�60     � RowDefinitions  5
   
   �� 5      $��45 �6   6*   �� 5      $��* �6   6)   .+�5   
   � 
������� � ColumnDefinitions 5      �� 5      6%   �� 5      6%   .+�5      �� $��10,0,0,0q�6   $��Center=�6*     list_all_table_of_checkout#���  6E   
���""��  Heading2#��[ 6�   6�   7  1FirstFloor.ModernUI.Windows.Controls.ModernButton  5      -   5          Name$ muiBtnExport��5   V   $��Center=�6   $��Right=�6:      EllipseDiameter$ 35Q�6l      
IconHeight$ 20Q�6�      	IconWidth$ 20Q�6�   $��1��6�   $��0,0,5,0q�6�      IconData� �pY�R��� @ѻ1�Jx~�Y�c`�Y�����Y����i=3]o�6� c`��J]��#@�"$2� :�� Y�;�� �        ы}}    Q�}}���}ҷ@Q�}}Y��zҷ @Q��=Y��zҷ @Q��=���}ҷ@1��,�@���}ҷ@�1��,�@Y6����=Y6����=!p}ы}}!p}ы}}Y6�ы}}������=����    ����    Y6�Q    ���}ҷ@	 � ToolTip 5      �� 5        export_table#���  6    6H   5      =  7FirstFloor.ModernUI.Windows.Controls.ModernProgressRing  5      -   5          Name$ 	mprExport��5   s   $��35��6%   $��35��60   $��Center=�6<   $��Right=�6W   $��1��6�   $��0,0,5,0q�6�   	   IsActive	 .   ! DoubleBounceProgressRingStyle#��[ 6�   6�   5      �� 5   
   $��1��6   $��	10,10,0,0q�6$   .��5      � $	��Top=�6     5      �� 5      $��40 �6$   62   �� 5       $��40 �6$   62   �� 5!      $��40 �6$   62   �� 5"      $��40 �6$   62   �� 5#      $��40 �6$   62   �� 5$      $��40 �6$   62   �� 5%      $��40 �6$   62   �� 5&      $��40 �6$   62   �� 5'      $��40 �6$   62   �� 5(      $��40 �6$   62   �� 5)      $��40 �6$   62   �� 5*      $��40 �6$   62   .+�5-      �� -   5        
 ��Name$
 chkCheckAll��5-      $��Center=�6p   $
��Bold"�6�   
���#�k�  	check_all#���  6�    5.      �� 5/      -   5         ��Name$ tblTooltipCheckAll��5/   $     select_all_table#���  6@   6l   51      �� 52      
�������$��Bottom=�64   6Q   �� 55      -   5        $
 chkTbCategory��55      $��Medium"�62     tb_category#���  6F   $��1��6n   $��Center=�6�    56      �� 57        select_this_table#���  6$   6Q   59      �� 5:      
�������$��Bottom=�64   $��1��6O   6^   �� 5=      -   5        $
 chkTbCustomer��5=      $��Medium"�62    	 tb_customer#��� 	 6F   $��2��6�   $��Center=�6�    5>      �� 5?      #���  6$   6Q   5A      �� 5B      
�������$��Bottom=�64   $��2��6O   6^   �� 5E      -   5        $
 chkTbInputHistory��5E      $��Medium"�66    
 tb_input_history#��� 
 6J   $��3��6�   $��Center=�6�    5F      �� 5G      #���  6$   6Q   5I      �� 5J      
�������$��Bottom=�64   $��3��6O   6^   �� 5M      -   5        $
 
chkTbOrder��5M      $��Medium"�6/     tb_order#���  6C   $��4��6�   $��Center=�6�    5N      �� 5O      #���  6$   6Q   5Q      �� 5R      
�������$��Bottom=�64   $��4��6O   6^   �� 5U      -	   5        $
 chkTbOrderDetail��5U      $��Medium"�65     tb_order_detail#���  6I   $��5��6�   $��Center=�6�    5V      �� 5W      #���  6$   6Q   5Y      �� 5Z      
�������$��Bottom=�64   $��5��6O   6^   �� 5]      -
   5        $
 chkTbPayment��5]      $��Medium"�61     
tb_payment#���  6E   $��6��6�   $��Center=�6�    5^      �� 5_      #���  6$   6Q   5a      �� 5b      
�������$��Bottom=�64   $��6��6O   6^   �� 5e      -   5        $
 chkTbProduct��5e      $��Medium"�61     
tb_product#���  6E   $��7��6�   $��Center=�6�    5f      �� 5g      #���  6$   6Q   5i      �� 5j      
�������$��Bottom=�64   $��7��6O   6^   �� 5m      -   5        $
 chkTbSalesperson��5m      $��Medium"�65     tb_salesperson#���  6I   $��8��6�   $��Center=�6�    5n      �� 5o      #���  6$   6Q   5q      �� 5r      
�������$��Bottom=�64   $��8��6O   6^   �� 5u      -   5        $
 chkTbSetting��5u      $��Medium"�61     
tb_setting#���  6E   $��9��6�   $��Center=�6�    5v      �� 5w      #���  6$   6Q   5y      �� 5z      
�������$��Bottom=�64   $��9��6O   6^   �� 5}      -   5        $
 	chkTbUser��5}      $��Medium"�6.     tb_user#���  6B   $��10��6�   $��Center=�6�    5~      �� 5      #���  6$   6Q   5�      �� 5�      
�������$��Bottom=�64   $��10��6O   6_   5�      5�      5�      5�      
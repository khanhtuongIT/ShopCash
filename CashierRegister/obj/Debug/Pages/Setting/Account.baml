   M S B A M L   `   `   `  ����  Cash Register+    %CashierRegister.Pages.Setting.Account   5      U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.8.0, Culture=neutral, PublicKeyToken=bc9b0c37bf06c6a90mui&http://firstfloorsoftware.com/ModernUI  5      -   5        .��5	      �    ContentRoot#��[  6   $��0q�60   $	��Top=�6;   $
��Left=�6S     � RowDefinitions  5
   
   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   6.   �� 5      $��* �6   6+   �� 5      $
��Auto �6   6.   .+�5   
   ��   personal_infomation#���  6   
����iA�  Heading1#��[ 6X   $��2��6z   �� 5   
   $��1��6   $��Medium"�6!   $��Center=�65   $��0 20 0 0q�6P     id#���  6b   6�   �� 5   
   -   5         ��Name$ tblID��5      $��1��6#   $��Center=�63   $��3 20 0 0q�6N   $��1��6`   6o   �� 5   
   $��2��6   $��Center=�6!   $��Medium"�6<   $��0 5 0 0q�6P     username#���  6a   6�   �� 5   
   -   5        $ tblUserName��5      $��Center=�6)   $��1��6D   $��3 5 0 0q�6T   $��2��6e   6t   �� 5!   
   $��3��6   $��Medium"�6!   $��Center=�65   $��0 5 0 0q�6P    	 email#���  6a   6�   �� 5"   
   -   5         ��Name$ txbEmail��5"       �� 	MaxLength$	 100��6$   $��Center=�64   $��25��6O   $��Center=�6[   $��1��6�   $��3 5 0 0q�6�   $��3��6�   $	��200��6�   6�   �� 5#   
   -   5         ��Name$ imgEmail��5#      $��Center=�6"   $y�Hidden=�6=   $��2��6Q   $��3��6a   $��20��6n   $��20��6y   $��3 0 0 0q�6�   6�   �� 5%   
   $��4��6   $��Center=�6!   $��0 5 0 0q�6<   $��Medium"�6M     address#���  6a   6�   �� 5&   
   -   5        $ 
txbAddress��5&      $	 250��6&   $��Center=�66   $��25��6Q   $��Center=�6]   $��3 5 0 0q�6�   $��1��6�   $��4��6�   $	��200��6�   6�   �� 5'   
   -   5        $ 
imgAddress��5'      $��Center=�6$   $y�Hidden=�6?   $��2��6S   $��4��6c   $��20��6p   $��20��6{   $��3 0 0 0q�6�   6�   �� 5)   
   $��5��6   $��Center=�6!   $��0 5 0 0q�6<   $��Medium"�6M     question#���  6a   6�   �� 5*   
   -   5        $ txbQuestion��5*      $	 200��6'   $��Center=�67   $��Center=�6R   $��25��6t   $��3 5 0 0q�6�   $��1��6�   $��5��6�   $	��200��6�   6�   �� 5+   
   -	   5        $ imgQuestion��5+      $��Center=�6%   $y�Hidden=�6@   $��2��6T   $��5��6d   $��20��6q   $��20��6|   $��3 0 0 0q�6�   6�   �� 5-   
   $��6��6   $��Center=�6!   $��0 5 0 0q�6<   $��Medium"�6M    
 answer#���  6a   6�   �� 5.   
   -
   5        $ 	txbAnswer��5.      $	 200��6%   $��25��65   $��Center=�6A   $��Center=�6c   $��3 5 0 0q�6�   $��1��6�   $��6��6�   $	��200��6�   6�   �� 5/   
   -   5        $ 	imgAnswer��5/      $y�Hidden=�6#   $��Center=�67   $��2��6R   $��6��6b   $��20��6o   $��20��6z   $��3 0 0 0q�6�   6�   �� 51   
   $��7��6   $��0,10,0,0q�6!   $��Right=�63   $��2��6O   �� 52      -   5         ��Name$ hplChangePassWord��52      .�6S   .�53      ��  	 change_password#��� 	 6$   6O   54      55      56      57      
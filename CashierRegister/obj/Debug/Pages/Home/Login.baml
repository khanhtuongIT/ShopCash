   M S B A M L   `   `   `  ����  Cash Register&     CashierRegister.Pages.Home.Login   5      U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.8.0, Culture=neutral, PublicKeyToken=bc9b0c37bf06c6a90mui&http://firstfloorsoftware.com/ModernUI  5      -   5        .��5	      �    ContentRoot#��[  6   $	��Top=�60   $��0q�6H   $��Center=�6S   .+�5
   
   � $	��350��6   $	��Top=�6     � RowDefinitions  5      �� 5      $
��Auto �6    60   �� 5      $
��Auto �6    60   �� 5      $
��Auto �6    60   �� 5      $
��Auto �6    60   �� 5      $
��Auto �6    60   �� 5      $
��Auto �6    60    � ColumnDefinitions 5      �� 5      $
��Auto �6#   62   �� 5      $��* �6#   6/   .+�5      ��   login_system#���  6   
����iA�  Heading1#��[ 6U   $��2��6w   $��Center=�6�   6�   =  7FirstFloor.ModernUI.Windows.Controls.ModernProgressRing  5      -   5          Name$	 mpr��5   %   $��1��62   $��2��6?   $��0 0 5 0q�6S      IsActive .     ThreeBounceProgressRingStyle#��[ 6u   6�   �� 5      -   5         ��Name$ tblNotification��5      $��1��61   $��Center=�6>   $��Center=�6[   $��2��6v   $��	0 20 0 10q�6�   
���""��6�   �� 5        username#���  6   $��Center=�6:   $��Medium"�6U   $��2��6i   $��0 0 0 5q�6v   6�   �� 5      -   5         ��Name$ txbUsername��5      $��3 0 0 5q�6+   $��25��6Z   $��Center=�6f   $��1��6�   $��2��6�   6�   �� 5         password#���  6   $��Center=�6:   $��3��6U   $��Medium"�6b   $��0 0 0 5q�6v   6�   G� 5!      -   5         G�Name$ pwbPassword��5!      $��3 0 0 5q�6/   $��2��6@   $��25��6P   $��Center=�6\   $��3��6�   6�   �� 5#      $��4��6   $��2��6%   $��Right=�69   $��0 0 0 15q�6U   $��Center=�6g   �� 5$      -   5         ��Name$ hplForgotPassword��5$      .�6X   ��   forgot_password#���  6b   6�   6�   5%      �� 5'       �� Orientation$ 
Horizontal=�6   $��Right=�62   $	��Top=�6N   $��5��6f   $��1��6s   .+�5(      �� -   5        	 ��Name$	 btnLogin��5(       	 login#���  6B   $��75��6d   $��0 0 5 0q�6r   6�   �� 5)      -   5        $	 	btnCancel��5)       
 cancel#���  6D   $��75��6g   6w   5*      5+      5,      5-      
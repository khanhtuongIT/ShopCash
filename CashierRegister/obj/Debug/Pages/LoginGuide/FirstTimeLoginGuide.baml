   M S B A M L   `   `   `  ����H  DShowAnimation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullD2clr-namespace:ShowAnimation;assembly=ShowAnimationShowAnimation  5       Cash Register:   4CashierRegister.Pages.LoginGuide.FirstTimeLoginGuide   5      U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.8.0, Culture=neutral, PublicKeyToken=bc9b0c37bf06c6a90mui&http://firstfloorsoftware.com/ModernUI  5      JContentTransition2clr-namespace:ShowAnimation;assembly=ShowAnimation  5      -   5        $	��450��5	   <   $	��625��6H      quick_guide  � Title#  �   5
        BlankWindow#��[ 5       � 
ResizeMode$ NoResize=�69    � WindowStyle$
 None=�6O   .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   6)   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   .+�5   
   � $��
10 10 10 0q�6    � ColumnDefinitions 5      �� 5      $��* �6#   6/   �� 5      $
��Auto �6#   62   �� 5      $��* �6#   6/   .+�5      %   ShowAnimation.ContentTransition  -   5        $��ContentTransition��5   2      TransitionType$ Slide=�6K   $��1��6b   $��2q�6r   6   5      �� 5   
   -   5         ��Name$ stpIndex��5      $��1��6'    �� Orientation$ 
Horizontal=�64   $��Bottom=�6M   $��Center=�6h   .+�5      � $��5 5 5 10q�6   .+�5      �� -   5         ��Name$
 img1��5      
	 J� Uid$	 1��6&   $
��None=�6]   6o   �� 5      -   5        
 ��Name$

 tbl1��5   %   $��1��6   $��Center=�6b   $��Center=�6}   
�������$��15(�6�   6�   5      � 5       $��5 5 5 10q�6   .+�5!      �� -   5        $
 img2��5!      $	 2��6&   $
��None=�6]   6o   �� 5"      -   5        $

 tbl2��5"   %   $��2��6   $��Center=�6b   $��Center=�6}   
�������$��15(�6�   6�   5#      � 5$      $��5 5 5 10q�6   .+�5%      �� -   5        $
 img3��5%      $	 3��6&   $
��None=�6]   6o   �� 5&      -	   5        $

 tbl3��5&   &   $��3��6   $��Center=�6c   $��Center=�6~   
�������$��15(�6�   6�   5'      � 5(      $��5 5 5 10q�6   .+�5)      �� -
   5        $
 img4��5)      $	 4��6&   $
��None=�6]   6o   �� 5*      -   5        $

 tbl4��5*   %   $��4��6   $��Center=�6b   $��Center=�6}   
�������$��15(�6�   6�   5+      5,      � 5-   
   $��2��6   $��3��6   .+�5.      �� -   5         ��Name$ chkShowGuide��5.        do_not_show_again#���  6-   $��Center=�6[   6x   �� 5/      -   5         ��Name$ btnReadMore��5/        	read_more#���  6*   $��Right=�6j   $��Center=�6�   6�   50      51      52      
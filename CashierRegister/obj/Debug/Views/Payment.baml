   M S B A M L   `   `   `  ����H  DCash Register, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:CashierRegister.ViewModelCashierRegister.ViewModel  5      A%clr-namespace:CashierRegister.HelpersCashierRegister.Helpers  5	      #    CashierRegister.Views.Payment   5      U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.8.0, Culture=neutral, PublicKeyToken=bc9b0c37bf06c6a90mui&http://firstfloorsoftware.com/ModernUI  5      7	viewModel'clr-namespace:CashierRegister.ViewModel  5      /vml%clr-namespace:CashierRegister.Helpers  5	      -   5        7  1FirstFloor.ModernUI.Windows.Controls.ModernDialog   Name$  
addWindows��5      .   (CashierRegister.Helpers.ViewModelLocator   AutoHookedUpViewModel . $
��1000��5   <   $
��1000��6M      payment � Title# �   5      .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   6)   �� 5      $
��Auto �6   6,   .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ 	dgPayment��5   3   $
��Left=�6F   $��5,20,0,0q�6a      AutoGenerateColumns .     HeadersVisibility$
 None=�6�      CanUserAddRows .  ��6   �� *	Payment+   Columns 5      4  .System.Windows.Controls.DataGridTemplateColumn  5      ,  &System.Windows.Controls.DataGridColumn	   Header$
	 Keep��6)   
   CellTemplate
 5      �� 5      .R�5      �� $��0,5,0,5q�6'   $	��200��68   ��6D   �� *PaymentName+ d� 	IsChecked 6d   ��  �� Path$ 
IsSelected� �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�5      5      5!      5#      �� 5%   
    �� Orientation$ 
Horizontal=�6   $��1��6.   $��Right=�6;   $��Bottom=�6W   $��0 50 0 0q�6r   .+�5&      � .+�5'      �� -   5         ��Name$ btnOkAddPayment��5'      $��5,0,0,0q�62     ok#���  6C   ��6b   �� *addPaymentCommand+��6�   ��  �� ElementName$ 
addWindows��6�   =  7FirstFloor.ModernUI.Windows.Controls.ModernProgressRing  5(      -   5          Name$ mprSaveSendEmail��5(   )      IsActive .  $y�Hidden=�6T      ThreeBounceProgressRingStyle#��[ 6h   6�   5)      �� 5*      -   5        $ btnClose��5*       
 cancel#���  6'   $��5,0,0,0q�6~   $��80��6�   ��6J   �� *CloseWindowCommand+$ OneWay=���6�   �� $ 
addWindows��6�   5,      5-      5/      
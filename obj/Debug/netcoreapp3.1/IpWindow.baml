   M S B A M L   `   `   `  ����J  Fbonnekesprinter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WPFCoreWPFCore  5   	       WPFCore.IpWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WPFCore     5   	     � Title$  Ip-adres instellen��5   	   $	��450��6$   $	��800��61   .��5	      �  � RowDefinitions 5
   
   �� 5      $��70 �6   6*   �� 5      $��60 �6   6*   �� 5      $��60 �6   6*   �� 5      $��60 �6   6*   �� 5      6    � ColumnDefinitions 5   
   �� 5      $	��200 �6   6-   �� 5      $	��150 �6   6-   �� 5      $	��170 �6   6-   �� 5      $	��150 �6   6-   �� 5      $��* �6   6+   .+�5   
   �� $��18(�6   $��Center=�6"   $��Center=�6?   $��1��6Z   $��1��6g   $��Ip-adres printer:��6w   6�   �� 5   
   -   5         ��Name$ 
ipAdresBox��5      $��30��6&   $	��140��62   $�� ��6>   $��1��6F   $��2��6S   6e   �� 5   
   -   5        $��40��5      $	��130��6   $��1��6)   $��3��66   .��6[   	Opslaan6d   �� 5   
   -   5         ��Name$ ipAdresBlock��5   1   $��Center=�6   
��� � �$��2��6Z   $��1��6g   $��3��6w   6�   �� 5   
   -   5        $��3��5   &   $��3��63   .��6C   Start printer6R   �� 5   
   $��18(�6   $��Center=�6"   $��Center=�6?   $��3��6Z   $��1��6g   Printer status:6�   �� 5   
   -   5        $ printerStatus��5   k   
���  ��$��18(�6%   $��Center=�63   $��Center=�6P   $��3��6�   $��2��6�   	offline6�   5      5       
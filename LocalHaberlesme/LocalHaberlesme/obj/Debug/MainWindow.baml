   M S B A M L   `   `   `  ����K  GLocal Haberlesme, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:LocalHaberlesmeLocalHaberlesme  5   	        LocalHaberlesme.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   U QMaterialDesignThemes.Wpf, Version=3.0.1.920, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   )localclr-namespace:LocalHaberlesme  5   	     � Title$  Oluştur / Bağlan��5	   	   $	��740��6"   $
��1346��6/    � WindowState$ Normal=�6<    � WindowStyle$
 None=�6Q   ���	#FF222324 � WindowStartupLocation$ CenterScreen=�6{    � 
ResizeMode$ NoResize=�6�   .��5
      �  � RowDefinitions 5   
   �� 5      .+�5   
   �� $��1q�6   $	��466��6%   $	��Top=�62   $��276,162,276,0q�6J    �� CornerRadius$ 40��6a   ���	#FF36393F._�5      � $��-1q�6    � ColumnDefinitions 5      �� 5      .+�5      � $��38q�6   .+�5       � -   5          �Name$ Pencere��5      $�� ��6-   $
��Left=�68   $	��718��6S   $	��391��6_   	  � NavigationUIVisibility$	 Hidden=�6l   $��0,0,0,-1q�6�   5      5      5      �� 5   
   $��1q�6   $��60��6%   $��526,84,526,0q�61   $	��Top=�6G   ���	#FF36393F$ 25��6v   ._�5      � .+�5      �� -   5        
 ��Name$
 olustursyfbtn��5      $��Sohbet Oluştur��60   $
��Left=�6I   $��	12,13,0,0q�6d   $	��125��6w   ���	#7F222324���	#FFFF9800���	#FF222324$
��Bold"�6�   $��36��6�   $	��Top=�6�   $��12(�6�   6  �� 5      -   5        $
 baglansyfbtn��5      $��Sohbete Bağlan��6/   $��157,13,10,0q�6H   ���	#7F222324$	��125��6t   ���	#FF222324���	#FFFF9800$
��Bold"�6�   $��36��6�   $	��Top=�6�   $��12(�6�   6�   5      5      � 5   
   -   5         �Name$ ustcubuk��5      $��25��6!   $	��Top=�6-   ���	#FF36393F$
��Left=�6\   $
��1346��6w   .+�5      �� -   5        $
 kapatbyn��5       &  "MaterialDesignFloatingActionButton#���   6'   $��
1321,2,0,0q�6d   $	��Top=�6x   $��20��6�   $��20��6�   ���	#FF222324$
��Left=�6�   .��5       '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ Shutdown=�6*   5!      �� 5"      -   5        $
 	kucultBtn��5"      #���   6(   $��
1291,2,0,0q�6e   $	��Top=�6y   $��20��6�   $��20��6�   ���	#FF222324$
��Left=�6�   .��5#        $ WindowMinimize=�6*   5$      �� 5%      -   5         ��Name$ baslik��5%      $'��!Sohbet Oluştur / Sohbete Bağlan��6$   $��37,-1,760,-4q�6N   ���	#FFFF9800$
��Bold"�6{   $��15(�6�   ��6�   Z� 5&      5'      5(      
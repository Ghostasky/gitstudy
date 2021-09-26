#include<QApplication>
#include<QWidget>//窗口控件基类


int main(int argc,char **argv)
{
    QApplication app(argc,argv);

    QWidget w;
    w.show();


    app.exec();



    return 0;
}

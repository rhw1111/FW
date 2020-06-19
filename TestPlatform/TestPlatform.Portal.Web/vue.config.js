module.exports = {

    /*
      2.1部署应用包时的基本URL。用法和webpack本身的output.publicPath一致。
  但在cli的其它地方也会用到这个值，所以请不要直接修改webpack的output.publicPath。
      2.2默认情况下,Vue Cli会假设你的应用是被部署在一个域名的根路径。
  如我本地的应用的路径是“D:\WORK\study\vue\vue_cli3_test\demo1”，则在这个应用中，根路径就是“D:\WORK\study\vue\vue_cli3_test\demo1”
    */
    publicPath: './',

    //3.当运行vue-cli-service build时生成的生产环境构建环境的目录。用法和webpack的output.path一样，不要修改output.path
    outputDir: 'dist',

    //4.放置打包后生成的静态资源（js、css、img、fonts）的目录，该目录相对于outputDir。
    assetsDir: './',

    //5.指定生成的index.html的输出路径，相对于outputDir。也可以是一个绝对路径。
    indexPath: 'index.html',

    //6.
    filenameHashing: true,

    //7.多页应用模式下构建应用
    pages: undefined,

    //8.安装@vue/cli-plugin-eslint后生效。为true时将检查错误输出为编译警告输出到命令行，编译不会失败。
    //为"error"时，将检查错误直接显示在浏览器中。强制eslint-loader将lint错误输出为编译错误，编译会失败。
    lintOnSave: true,

    //9.
    //tuntimeCompiler: false,
    //10.
    transpileDependencies: [
      'quasar'
    ],
    //11.如果你不需要生产环境的 source map，可以将其设置为 false 以加速生产环境构建
    productionSourceMap: true,
    //12.
    crossorigin: undefined,
    //13.
    integrity: false,
    //14.
    parallel: require('os').cpus().length > 1,
    //15.向PWA插件传递选项
    pwa: {},
    //16.不进行任何schema验证的对象，可以用来传递任何第三方插件选项，不是webpack的plugins
    pluginOptions: {
      quasar: {
        importStrategy: 'manual',
        rtlSupport: true
      }
    },

    //17.和wenpack相关的配置参考最上面代码
}

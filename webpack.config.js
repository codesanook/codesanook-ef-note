const path = require("path");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

const entryFile = [__dirname, "src", "css", "site"];
const outputPath = [__dirname, "wwwroot", "css"];

module.exports = {
  entry: {
    site: path.resolve(...entryFile)
  },
  output: {
    path: path.resolve(...outputPath),
    filename: "_[name].css"
  },
  resolve: {
    extensions: [".scss"]
  },
  module: {
    rules: [
      {
        test: /\.scss$/,
        use: [
          {
            loader: MiniCssExtractPlugin.loader
          },
          {
            loader: "css-loader" // translates CSS into CommonJS modules
          },
          {
            loader: "postcss-loader", // Run post css actions
            options: {
              plugins: () => {
                // post css plugins, can be exported to postcss.config.js
                return [require("precss"), require("autoprefixer")];
              }
            }
          },
          {
            loader: "resolve-url-loader"
          },
          {
            loader: "sass-loader", // compiles Sass to CSS, using Node Sass by default
            options: {
              sourceMap: true
            }
          }
        ],
        exclude: /node_modules/
      },
      {
        test: /\.(png|jpe?g|gif|svg|eot|ttf|woff2?)$/,
        use: [
          {
            loader: "file-loader",
            options: {
              name: "[name].[ext]",
              outputPath: "." //relative to output
            }
          }
        ]
      }
    ]
  },
  plugins: [
    new MiniCssExtractPlugin({
      filename: "./site.css" //relative to output
    })
  ]
};

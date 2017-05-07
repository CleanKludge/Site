/// <binding AfterBuild='clean:css, less, min:css, min:js' Clean='clean:js, clean:css' />
"use strict";

var gulp = require("gulp"),
    del = require("del"),
    concat = require("gulp-concat"),
    rename = require("gulp-rename"),
    cssmin = require("gulp-cssmin"),
    htmlmin = require("gulp-htmlmin"),
    uglify = require("gulp-uglify"),
    less = require("gulp-less"),
    config = require("./appsettings.json"),
    bundles = config.Bundles;

gulp.task("clean:js", function () {
    return del([bundles.Js.Minified]);
});

gulp.task("clean:css", function () {
    return del([bundles.Css.Source]);
});

gulp.task("min:js", function () {
    return gulp.src([bundles.Js.Source, "!" + bundles.Js.Minified])
        .pipe(uglify())
        .pipe(rename({ suffix: ".min" }))
        .pipe(gulp.dest(bundles.Js.Destination));
});

gulp.task("min:css", function () {
    return gulp.src([bundles.Css.Source, "!" + bundles.Css.Minified])
        .pipe(cssmin())
        .pipe(rename({ suffix: ".min" }))
        .pipe(gulp.dest(bundles.Css.Destination));
});

gulp.task("less", function () {
    return gulp.src([bundles.Less.Source, "!" + bundles.Less.Base])
        .pipe(less())
        .pipe(gulp.dest(bundles.Less.Destination));
});;
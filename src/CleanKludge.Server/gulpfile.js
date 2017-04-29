/// <binding AfterBuild='clean:css, less, min:css, min:js' Clean='clean:js, clean:css' />
"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    rename = require("gulp-rename"),
    cssmin = require("gulp-cssmin"),
    htmlmin = require("gulp-htmlmin"),
    uglify = require("gulp-uglify"),
    less = require("gulp-less"),
    config = require("./appsettings.json"),
    bundles = config.Bundles;

gulp.task("clean:js", function (cb) {
    rimraf(bundles.Js.Destination, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(bundles.Css.Destination, cb);
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
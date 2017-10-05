module.exports = function (grunt) {
    grunt.initConfig({
        clean: {
            debug: ['bin', 'dist'],
            release: ['bin', 'dist']
        },
        copy: {
            debug: {
                files: [
                ]
            },
            release: {
                files: [
                ]
            }
        },
    });
    grunt.loadNpmTasks('grunt-contrib-clean');
    grunt.loadNpmTasks('grunt-contrib-copy');
};

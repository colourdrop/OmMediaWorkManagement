/*! 
Name: jQuery grouploop plugin
Verison: 1.0.3
Author: Scott Alguire
Repository: https://github.com/scottalguire/grouploop
*/
! function(f) {
    f.fn.extend({
        grouploop: function(i) {
            var n, e, t, r = f.extend({
                    velocity: 2,
                    forward: !0,
                    childNode: ".item",
                    childWrapper: ".item-wrap",
                    pauseOnHover: !0,
                    complete: null,
                    stickFirstItem: !1
                }, i),
                o = this,
                d = f(window).width() < 768 ? 2 * f(window).width() : f(window).width(),
                c = r.velocity,
                a = f(window).width() < 768 ? 2 * f(o).width() : f(o).width(),
                s = f(o).find(r.childWrapper + " " + r.childNode).first(),
                p = f(o).find(r.childWrapper + " " + r.childNode).length;

            function h() {
                e = a / (p - 1), o.css("position", "relative"), s.remove(), s.css({
                    position: "absolute",
                    top: "0",
                    left: "0",
                    width: e,
                    height: "100%",
                    "z-index": "999"
                }), s.prependTo(o).find(r.childWrapper)
            }

            function l() {
                r.forward ? n <= 0 ? (n += 1 * c, f(o).find(r.childWrapper).css("transform", "translateX(" + n + "px)")) : (f(o).find(r.childWrapper).css("transform", "translateX(" + -d - e + ")"), n = -d) : -d <= n ? (n -= 1 * c, f(o).find(r.childWrapper).css("transform", "translateX(" + n + "px)")) : (f(o).find(r.childWrapper).css("transform", "translateX(0)"), n = 0), t = window.requestAnimationFrame(l)
            }
            return r.stickFirstItem ? h() : e = 0, window.addEventListener("resize", function() {
                (d = f(window).width()) < 768 ? (console.log("Small breakpoint. Wrapper width is currently doubled."), d *= 2, a = 2 * f(o).width()) : (d = f(window).width(), a = f(o).width()), r.stickFirstItem && h()
            }), r.forward ? (n = -d, f(o).each(function(i, n) {
                f(n).find(r.childWrapper).each(function(i, n) {
                    f(f(n).find(f(r.childNode).get().reverse())).each(function() {
                        f(this).clone().prependTo(n)
                    })
                })
            })) : (n = "none" !== f(o).find(r.childWrapper).css("transform") ? f(o).find(r.childWrapper).css("transform").split(/[()]/)[1].split(",")[4] : 0, f(o).each(function(i, n) {
                f(n).find(r.childWrapper).each(function(i, n) {
                    f(n).find(r.childNode).each(function() {
                        f(this).clone().appendTo(n)
                    })
                })
            })), t = window.requestAnimationFrame(l), r.pauseOnHover && f(this).hover(function() {
                cancelAnimationFrame(t)
            }, function() {
                t = window.requestAnimationFrame(l)
            }), this.each(function() {
                f.isFunction(r.complete) && r.complete.call(this)
            })
        }
    })
}(jQuery);
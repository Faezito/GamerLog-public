/**
 * GAMERLOG DESIGN SYSTEM — gamerlog.js
 * Arquivo: wwwroot/js/gamerlog.js
 *
 * Módulos:
 *  1.  GlCursor        — Cursor personalizado com lag suave
 *  2.  GlReveal        — Scroll reveal com IntersectionObserver
 *  3.  GlXpBar         — Anima XP bars declarativas via data-*
 *  4.  GlMiniChart     — Mini gráficos de barras animados
 *  5.  GlAchievement   — Toasts de achievement (programático e declarativo)
 *  6.  GlPixelCanvas   — Canvas de pixel art decorativo
 *  7.  GlHudDate       — Preenche datas HUD automaticamente
 *  8.  GlModal         — Atalhos de teclado e helpers para modais Bootstrap
 *  9.  GlFormAjax      — Wrapper AJAX padronizado para forms do sistema
 *  10. GlInit          — Bootstrap automático de todos os módulos
 */

(function (window, document, $) {
    'use strict';

    /* ═══════════════════════════════════════════════════════════════
       UTILIDADES INTERNAS
    ═══════════════════════════════════════════════════════════════ */
    function qs(sel, ctx)  { return (ctx || document).querySelector(sel); }
    function qsa(sel, ctx) { return (ctx || document).querySelectorAll(sel); }

    /* ═══════════════════════════════════════════════════════════════
       1. GlCursor — Cursor personalizado
       HTML necessário:
         <div id="cursor"></div>
         <div id="cursor-ring"></div>
    ═══════════════════════════════════════════════════════════════ */
    var GlCursor = {
        cursor: null,
        ring: null,
        mx: 0, my: 0,
        rx: 0, ry: 0,

        init: function () {
            this.cursor = qs('#cursor');
            this.ring   = qs('#cursor-ring');
            if (!this.cursor || !this.ring) return;

            document.addEventListener('mousemove', function (e) {
                GlCursor.mx = e.clientX;
                GlCursor.my = e.clientY;
            });

            this._loop();
        },

        _loop: function () {
            GlCursor.cursor.style.left = GlCursor.mx + 'px';
            GlCursor.cursor.style.top  = GlCursor.my + 'px';

            GlCursor.rx += (GlCursor.mx - GlCursor.rx) * 0.12;
            GlCursor.ry += (GlCursor.my - GlCursor.ry) * 0.12;
            GlCursor.ring.style.left = GlCursor.rx + 'px';
            GlCursor.ring.style.top  = GlCursor.ry + 'px';

            requestAnimationFrame(GlCursor._loop);
        }
    };

    /* ═══════════════════════════════════════════════════════════════
       2. GlReveal — Scroll reveal
       Uso: adicione .gl-reveal em qualquer elemento.
            Quando entrar na viewport, recebe .visible
    ═══════════════════════════════════════════════════════════════ */
    var GlReveal = {
        init: function () {
            var els = qsa('.gl-reveal');
            if (!els.length) return;

            if (!('IntersectionObserver' in window)) {
                els.forEach(function (el) { el.classList.add('visible'); });
                return;
            }

            var io = new IntersectionObserver(function (entries) {
                entries.forEach(function (entry) {
                    if (entry.isIntersecting) {
                        entry.target.classList.add('visible');
                    }
                });
            }, { threshold: 0.1 });

            els.forEach(function (el) { io.observe(el); });
        }
    };

    /* ═══════════════════════════════════════════════════════════════
       3. GlXpBar — XP / Progress bars animadas
       Uso: <div class="gl-xp-track" data-xp="73" data-total="100">
              <div class="gl-xp-fill"></div>
            </div>
            <span class="gl-xp-counter" data-target="730" data-max="1000">0</span>
    ═══════════════════════════════════════════════════════════════ */
    var GlXpBar = {
        DELAY: 800,

        init: function () {
            var tracks = qsa('[data-xp]');
            if (!tracks.length) return;

            setTimeout(function () {
                tracks.forEach(function (track) {
                    var pct   = parseFloat(track.dataset.xp)    || 0;
                    var total = parseFloat(track.dataset.total)  || 100;
                    var fill  = track.querySelector('.gl-xp-fill');
                    if (!fill) return;

                    fill.style.width = ((pct / total) * 100).toFixed(1) + '%';
                });

                GlXpBar._animateCounters();
            }, GlXpBar.DELAY);
        },

        _animateCounters: function () {
            qsa('[data-xp-counter]').forEach(function (el) {
                var target  = parseInt(el.dataset.xpCounter) || 0;
                var max     = parseInt(el.dataset.xpMax)     || 1000;
                var step    = Math.ceil(target / 60);
                var current = 0;

                var interval = setInterval(function () {
                    current = Math.min(current + step, target);
                    el.textContent = current + ' / ' + max + ' XP';
                    if (current >= target) clearInterval(interval);
                }, 30);
            });
        }
    };

    /* ═══════════════════════════════════════════════════════════════
       4. GlMiniChart — Mini gráficos de barras
       Uso: <div class="gl-mini-bars" data-heights="40,65,50,80,45,90,70">
              <!-- barras geradas via JS, ou já no HTML -->
            </div>
    ═══════════════════════════════════════════════════════════════ */
    var GlMiniChart = {
        init: function () {
            qsa('.gl-mini-bars[data-heights]').forEach(function (wrap) {
                var heights = wrap.dataset.heights.split(',');
                wrap.innerHTML = '';

                heights.forEach(function (h, i) {
                    var bar = document.createElement('div');
                    bar.className = 'gl-mini-bar';
                    bar.style.height = parseFloat(h) + '%';
                    bar.style.animationDelay = (i * 0.05) + 's';

                    // Último bar destaque
                    if (i === heights.length - 2) bar.classList.add('active');

                    wrap.appendChild(bar);
                });
            });
        }
    };

    /* ═══════════════════════════════════════════════════════════════
       5. GlAchievement — Toasts de conquista
       Uso programático:
         GlAchievement.show({ icon:'🏆', name:'Primeiro Log', desc:'...', delay: 2800 })

       Uso declarativo (auto-show na página):
         <div class="gl-achievement-toast" data-show-delay="2800">...</div>
    ═══════════════════════════════════════════════════════════════ */
    var GlAchievement = {
        HIDE_AFTER: 4500,

        /**
         * Exibe um toast de achievement
         * @param {Object} opts - { toastId, icon, name, desc, delay, hideAfter }
         */
        show: function (opts) {
            opts = opts || {};

            if (opts.toastId) {
                // Usa um toast existente no DOM
                var el = qs('#' + opts.toastId);
                if (!el) return;
                setTimeout(function () {
                    el.classList.add('show');
                    setTimeout(function () { el.classList.remove('show'); }, opts.hideAfter || GlAchievement.HIDE_AFTER);
                }, opts.delay || 0);
                return;
            }

            // Cria toast dinamicamente
            var toast = document.createElement('div');
            toast.className = 'gl-achievement-toast';
            toast.innerHTML =
                '<div class="gl-ach-shine"></div>' +
                '<div class="gl-ach-icon-sm">' + (opts.icon || '🏆') + '</div>' +
                '<div>' +
                    '<div class="ach-label">Achievement Unlocked</div>' +
                    '<div class="ach-name">' + (opts.name || '') + '</div>' +
                    '<div class="ach-desc">' + (opts.desc || '') + '</div>' +
                '</div>';

            document.body.appendChild(toast);

            setTimeout(function () {
                toast.classList.add('show');
                setTimeout(function () {
                    toast.classList.remove('show');
                    setTimeout(function () { toast.remove(); }, 600);
                }, opts.hideAfter || GlAchievement.HIDE_AFTER);
            }, opts.delay || 100);
        },

        /* Auto-inicializa toasts declarativos com data-show-delay */
        init: function () {
            qsa('.gl-achievement-toast[data-show-delay]').forEach(function (el) {
                var delay = parseInt(el.dataset.showDelay) || 0;
                GlAchievement.show({ toastId: el.id, delay: delay });
            });
        }
    };

    /* ═══════════════════════════════════════════════════════════════
       6. GlPixelCanvas — Canvas decorativo de pixels
       Uso: <div class="gl-pixel-bg">
              <canvas id="pixelCanvas"></canvas>
            </div>
       Ou com seletor customizado: GlPixelCanvas.init('#meuCanvas')
    ═══════════════════════════════════════════════════════════════ */
    var GlPixelCanvas = {
        PIXEL_SIZE: 16,
        DENSITY:    0.88, // probabilidade de NÃO desenhar (quanto maior, menos pixels)

        init: function (selector) {
            var sel = selector || '#pixelCanvas';
            var cvs = qs(sel);
            if (!cvs) return;

            var ctx = cvs.getContext('2d');

            function resize() {
                cvs.width  = cvs.offsetWidth;
                cvs.height = cvs.offsetHeight;
                GlPixelCanvas.draw(ctx, cvs.width, cvs.height);
            }

            window.addEventListener('resize', resize);
            resize();
        },

        draw: function (ctx, w, h) {
            var sz = GlPixelCanvas.PIXEL_SIZE;
            ctx.clearRect(0, 0, w, h);

            for (var x = 0; x < w; x += sz) {
                for (var y = 0; y < h; y += sz) {
                    if (Math.random() > GlPixelCanvas.DENSITY) {
                        var a = Math.random() * 0.5 + 0.1;
                        ctx.fillStyle = Math.random() > 0.5
                            ? 'rgba(6,182,212,' + a + ')'
                            : 'rgba(37,99,235,' + a + ')';
                        ctx.fillRect(x, y, sz - 1, sz - 1);
                    }
                }
            }
        }
    };

    /* ═══════════════════════════════════════════════════════════════
       7. GlHudDate — Preenche datas com formato pt-BR
       Uso: <span class="gl-hud-date"></span>
            ou <span id="minhaData" class="gl-hud-date" data-format="short"></span>
    ═══════════════════════════════════════════════════════════════ */
    var GlHudDate = {
        init: function () {
            qsa('.gl-hud-date').forEach(function (el) {
                var fmt = el.dataset.format === 'long' ? {
                    weekday: 'short', year: 'numeric', month: 'short', day: 'numeric'
                } : undefined;

                el.textContent = new Date().toLocaleDateString('pt-BR', fmt);
            });
        }
    };

    /* ═══════════════════════════════════════════════════════════════
       8. GlModal — Helpers para modais Bootstrap 5
       • Fecha modal com ESC (além do comportamento nativo do Bootstrap)
       • Adiciona .gl-modal-loading ao body quando modal abre
    ═══════════════════════════════════════════════════════════════ */
    var GlModal = {
        init: function () {
            // Garante que ESC fecha modais Bootstrap normalmente
            document.addEventListener('keydown', function (e) {
                if (e.key !== 'Escape') return;
                var open = qs('.modal.show');
                if (!open) return;
                var instance = bootstrap && bootstrap.Modal.getInstance(open);
                if (instance) instance.hide();
            });
        },

        /**
         * Abre um modal Bootstrap pelo id
         * @param {string} modalId
         */
        open: function (modalId) {
            var el = qs('#' + modalId);
            if (!el) return;
            var m = new bootstrap.Modal(el);
            m.show();
        },

        /**
         * Fecha um modal Bootstrap pelo id
         * @param {string} modalId
         */
        close: function (modalId) {
            var el = qs('#' + modalId);
            if (!el) return;
            var instance = bootstrap.Modal.getInstance(el);
            if (instance) instance.hide();
        }
    };

    /* ═══════════════════════════════════════════════════════════════
       9. GlFormAjax — Wrapper AJAX padronizado (jQuery + SweetAlert2)
       Uso:
         GlFormAjax.bind('#form-login', {
           url:     '/Home/Login',
           method:  'POST',
           btnId:   '#btnLogin',
           spinnerId: '#loading-login',
           labelId:   '#texto-login',
           onSuccess: function(res) { window.location.href = res.redirectUrl; }
         });
    ═══════════════════════════════════════════════════════════════ */
    var GlFormAjax = {
        /**
         * @param {string}   formSel  - Seletor do form
         * @param {Object}   opts
         * @param {string}   opts.url
         * @param {string}   [opts.method='POST']
         * @param {string}   [opts.btnId]
         * @param {string}   [opts.spinnerId]
         * @param {string}   [opts.labelId]
         * @param {Function} [opts.onSuccess]
         * @param {Function} [opts.onError]
         * @param {Object}   [opts.extraData]  - Dados adicionais merged no serialize
         */
        bind: function (formSel, opts) {
            if (!$ || !$.ajax) {
                console.warn('GlFormAjax: jQuery não encontrado.');
                return;
            }

            opts = opts || {};

            $(document).on('submit', formSel, function (e) {
                e.preventDefault();

                var $form    = $(this);
                var $btn     = opts.btnId     ? $(opts.btnId)     : $form.find('[type=submit]');
                var $spinner = opts.spinnerId ? $(opts.spinnerId) : null;
                var $label   = opts.labelId   ? $(opts.labelId)   : null;

                var data = $form.serialize();
                if (opts.extraData) {
                    data += '&' + $.param(opts.extraData);
                }

                $.ajax({
                    url:     opts.url    || $form.attr('action'),
                    type:    opts.method || 'POST',
                    data:    data,
                    headers: { 'X-Requested-With': 'XMLHttpRequest' },

                    beforeSend: function () {
                        $btn.prop('disabled', true);
                        if ($spinner) $spinner.removeClass('d-none');
                        if ($label)   $label.hide();
                    },

                    complete: function () {
                        $btn.prop('disabled', false);
                        if ($spinner) $spinner.addClass('d-none');
                        if ($label)   $label.show();
                    },

                    success: function (res) {
                        if (typeof opts.onSuccess === 'function') {
                            opts.onSuccess(res);
                        } else {
                            GlFormAjax._defaultSuccess(res);
                        }
                    },

                    error: function (xhr) {
                        if (typeof opts.onError === 'function') {
                            opts.onError(xhr);
                        } else {
                            GlFormAjax._defaultError(xhr);
                        }
                    }
                });
            });
        },

        /**
         * Bind para um botão que dispara AJAX com confirmação SweetAlert2
         * @param {string}   btnSel
         * @param {Object}   opts  — mesmos de .bind(), mais:
         *   opts.confirm: { title, denyText, confirmText }
         *   opts.getData: function() → string (serialização)
         */
        bindConfirm: function (btnSel, opts) {
            if (!$ || !window.Swal) {
                console.warn('GlFormAjax.bindConfirm: jQuery ou SweetAlert2 não encontrado.');
                return;
            }

            opts = opts || {};
            var confirm = opts.confirm || {};

            $(document).on('click', btnSel, function (e) {
                e.preventDefault();
                var $btn = $(this);

                Swal.fire({
                    title:          confirm.title       || 'Tem certeza?',
                    showDenyButton: true,
                    denyButtonText: confirm.denyText    || 'Não',
                    confirmButtonText: confirm.confirmText || 'Sim'
                }).then(function (result) {
                    if (!result.isConfirmed) return;

                    var data = typeof opts.getData === 'function'
                        ? opts.getData()
                        : (opts.formSel ? $(opts.formSel).serialize() : '');

                    var $spinner = opts.spinnerId ? $(opts.spinnerId) : null;
                    var $label   = opts.labelId   ? $(opts.labelId)   : null;

                    $.ajax({
                        url:     opts.url,
                        type:    opts.method || 'POST',
                        data:    data,
                        headers: { 'X-Requested-With': 'XMLHttpRequest' },

                        beforeSend: function () {
                            $btn.prop('disabled', true);
                            if ($spinner) $spinner.removeClass('d-none');
                            if ($label)   $label.hide();
                        },

                        complete: function () {
                            $btn.prop('disabled', false);
                            if ($spinner) $spinner.addClass('d-none');
                            if ($label)   $label.show();
                        }
                    })
                    .done(function (res) {
                        if (typeof opts.onSuccess === 'function') {
                            opts.onSuccess(res);
                        } else {
                            GlFormAjax._defaultSuccess(res);
                        }
                    })
                    .fail(function (xhr) {
                        if (typeof opts.onError === 'function') {
                            opts.onError(xhr);
                        } else {
                            GlFormAjax._defaultError(xhr);
                        }
                    });
                });
            });
        },

        _defaultSuccess: function (res) {
            if (!window.Swal) return;
            if (res.success) {
                if (res.redirectUrl) { window.location.href = res.redirectUrl; return; }
                Swal.fire({ icon: 'success', title: res.title || 'Sucesso', text: res.detail });
            } else {
                Swal.fire({ icon: 'error', title: res.title || 'Erro', text: res.detail });
            }
        },

        _defaultError: function (xhr) {
            if (!window.Swal) return;
            var j = xhr.responseJSON;
            Swal.fire({
                icon:  'error',
                title: j && j.title  ? j.title  : 'Erro',
                text:  j && j.detail ? j.detail : 'Ocorreu um erro inesperado.'
            });
        }
    };

    /* ═══════════════════════════════════════════════════════════════
       10. GlInit — Bootstrap automático
       Inicializa todos os módulos quando o DOM estiver pronto.
       Pode ser desabilitado: window.GL_NO_AUTO_INIT = true
    ═══════════════════════════════════════════════════════════════ */
    var GlInit = {
        run: function () {
            GlCursor.init();
            GlReveal.init();
            GlXpBar.init();
            GlMiniChart.init();
            GlAchievement.init();
            GlPixelCanvas.init();
            GlHudDate.init();
            GlModal.init();
        }
    };

    /* ─────────────────────────────────────────
       Exposição da API pública no objeto global
    ───────────────────────────────────────── */
    window.GL = {
        Cursor:      GlCursor,
        Reveal:      GlReveal,
        XpBar:       GlXpBar,
        MiniChart:   GlMiniChart,
        Achievement: GlAchievement,
        PixelCanvas: GlPixelCanvas,
        HudDate:     GlHudDate,
        Modal:       GlModal,
        FormAjax:    GlFormAjax
    };

    /* ─────────────────────────────────────────
       Auto-init on DOMContentLoaded
    ───────────────────────────────────────── */
    if (window.GL_NO_AUTO_INIT) return;

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', GlInit.run);
    } else {
        GlInit.run();
    }

}(window, document, window.jQuery));

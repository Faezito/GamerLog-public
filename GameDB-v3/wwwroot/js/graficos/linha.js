function fn_1_linha_min_max_media(id, xAxis_data, series_data) {

    var chartDom = document.getElementById(id);
    var myChart = echarts.init(chartDom);
    var option;

    option = {
        grid: {
            left: '1%',
            right: '0%',
            top: '27%',
            bottom: '0%',
            height: "70%",
            width: "95%",
            containLabel: true
        },
        xAxis: {
            type: 'category',
            data: xAxis_data
        },
        yAxis: {
            show: true,
            type: 'value',
            axisLabel: {
                formatter: function (value, index) {
                    return value.toLocaleString('pt-br')
                }
            }
        },
        legend: {
            label: {
                formatter: function (valor) {
                    return valor.value.toLocaleString('pt-br');
                }
            }
        },
        series: [
            {
                data: series_data,
                label: {
                    show: true,
                    formatter: function (valor) {
                        return valor.value.toLocaleString('pt-br');
                    }
                },
                markPoint: {
                    silent: true,
                    label: {
                        show: false
                    },
                    data: [
                        {
                            type: 'max',
                            name: 'Max',
                            itemStyle: {
                                color: 'darkgreen'
                            },
                            symbol: "arrow",
                            symbolSize: 30,
                            symbolRotate: 180,
                            symbolOffset: [0, -20]
                        },
                        {
                            type: 'min',
                            name: 'Min',
                            itemStyle: {
                                color: 'darkred'
                            },
                            symbol: "arrow",
                            symbolSize: 30,
                            symbolRotate: 180,
                            symbolOffset: [0, -20]
                        }
                    ]
                },
                markLine: {
                    data: [{ type: 'average', name: 'Avg' }],
                    label: {
                        formatter: function (valor) {
                            return valor.value.toLocaleString('pt-br');
                        }
                    }
                },
                type: 'line',
                smooth: false
            }
        ]
    };

    option && myChart.setOption(option);
}

function fn_linha_faturamento_diario(id, dias, loja, cafe, total) {
    let el = document.getElementById(id);
    let myChart = echarts.init(el);
    let option;

    option = {
        tooltip: {
            trigger: 'axis'
        },
        legend: {
            data: ['Loja', 'Café', 'Total']
        },
        grid: {
            left: '2%',
            right: '2%',
            bottom: '20%',
            containLabel: false
        },
        //toolbox: {
        //    feature: {
        //        saveAsImage: {}
        //    }
        //},
        xAxis: {
            type: 'category',
            boundaryGap: false,
            data: dias
        },
        yAxis: {
            type: 'value',
            axisLabel: {
                show: false
            }
        },
        series: [
            {
                name: 'Loja',
                type: 'line',
                smooth: false,
                data: loja
            },
            {
                name: 'Café',
                type: 'line',
                smooth: false,
                data: cafe
            },
            {
                name: 'Total',
                type: 'line',
                color: 'rgb(255, 99, 71)',
                smooth: false,
                data: total,
                label: {
                    show: true,
                    position: 'top',
                    formatter: function (params) {
                        return Math.round(params.value); // Remove os decimais
                    }
                }
            }
        ]
    };

    myChart.setOption(option);
}

function fn_linha_sazonal(id, dias, valores, anos) {
    let el = document.getElementById(id);
    let myChart = echarts.init(el);
    let option;

    option = {
        tooltip: {
            trigger: 'axis',
            valueFormatter: function (value) {
                return value.toLocaleString('pt-BR');
            }
        },
        legend: {
            data: anos
        },
        grid: {
            left: '2%',
            right: '2%',
            bottom: '35%',
            containLabel: false
        },
        xAxis: {
            type: 'category',
            boundaryGap: false,
            data: dias,
            axisLabel: {
                rotate: 90
            }
        },
        yAxis: {
            type: 'value',
            axisLabel: {
                show: false
            }
        },
        series: [
            {
                name: anos[0],
                type: 'line',
                smooth: false,
                data: valores[0]
            },
            {
                name: anos[1],
                type: 'line',
                smooth: false,
                data: valores[1],
                label: {
                    show: true,
                    position: 'top',
                    offset: [0, -17],
                    formatter: function (params) {
                        return Math.round(params.value); // Remove os decimais
                    }
                }
            }
        ]
    };

    myChart.setOption(option);
}